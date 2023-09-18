using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Index
{
    internal partial class DataNode<TKey> : IDisposable where TKey : IComparable
    {
        private INodeDataContext<TKey> _nodeDataSource;
        private SortedList<TKey, NodeItem<TKey>> _items;
        private int _branchingFactor;
        private long? _parentAddress;
               

        public DataNode(INodeDataContext<TKey> nodeDataSource, int branchingFactor)
        {
            _nodeDataSource = nodeDataSource;
            _items = new SortedList<TKey, NodeItem<TKey>>();
            _branchingFactor = branchingFactor;
        }        

        public async Task Insert(TKey key, long value)
        {
            var root = await _nodeDataSource.ReadRootNode().ConfigureAwait(false);
            await root.InsertInternal(key, value).ConfigureAwait(false);
        }

        public async Task<long> Query(TKey key)
        {
            var root = await _nodeDataSource.ReadRootNode().ConfigureAwait(false);
            return await root.QueryInternal(key).ConfigureAwait(false);
        }

        public Task Flush()
        {
            return _nodeDataSource.Flush();
        }

        private async Task<long> QueryInternal(TKey key)
        {
            if (_items.Count == 0)
                return -1;

            var relevantItem = GetRelevantItem(key);
            if (relevantItem == null)
                return -1;
                //throw new Exception($"no node item available for key {key}");

            if (IsLeafNode())
            {
                if (relevantItem.Key.CompareTo(key) == 0)
                    return relevantItem.Value;
                else
                    return -1;

            }

            var childNode = await _nodeDataSource.Read(relevantItem.DataAddress.Value).ConfigureAwait(false);
            if (childNode == null)
                throw new Exception($"No child available for key {key} at address {relevantItem.DataAddress}");
            return await childNode.QueryInternal(key).ConfigureAwait(false);
        }

        private async Task InsertInternal(TKey key, long value)
        {
            if (!IsLeafNode())
            {
                var item = GetRelevantItem(key);
                var child = await _nodeDataSource.Read(item.DataAddress.Value).ConfigureAwait(false);
                await child.InsertInternal(key, value).ConfigureAwait(false); 
            }
            else
            {
                NodeModified = true;
                if (_items.ContainsKey(key))
                {
                    _items[key].Value = value;
                }
                else
                {
                    _items.Add(key, new NodeItem<TKey>
                    {
                        Key = key,
                        Value = value,
                        IsLeaf = true
                    });

                    if (IsFull())
                    {
                        await SplitNode().ConfigureAwait(false);
                    }
                }
                await WriteNodeToDisk().ConfigureAwait(false);
            }
        }

        private async Task SplitNode()
        {
            var splitIndex = _items.Count / 2;
            var newNode = new DataNode<TKey>(_nodeDataSource, _branchingFactor);
            newNode.NodeModified = true;
            var newNodeAddress = await newNode.WriteNodeToDisk().ConfigureAwait(false);
            var rightItems = _items.Skip(splitIndex).ToList();            
            _items = new SortedList<TKey, NodeItem<TKey>>(_items.Values.Take(splitIndex).ToDictionary(x => x.Key));
            NodeModified = true;
            newNode.NodeModified = true;
            foreach (var item in rightItems)
            {
                newNode._items.Add(item.Key, item.Value);
                if (!item.Value.IsLeaf)
                {
                    var childNode = await _nodeDataSource.Read(item.Value.DataAddress.Value).ConfigureAwait(false);
                    childNode._parentAddress = newNodeAddress;
                    childNode.NodeModified = true;
                }
            }            

            var newNodeItem = new NodeItem<TKey>
            {
                Key = rightItems.Min(x => x.Value.Key),
                DataAddress = newNodeAddress,
                IsLeaf = false
            };

            var leftItem = new NodeItem<TKey>
            {
                Key = _items.Min(x => x.Key),
                DataAddress = NodeAddress,
                IsLeaf = false
            };

            var parent = await BubbleUpToParent(leftItem, newNodeItem).ConfigureAwait(false);
            _parentAddress = parent.NodeAddress;
            newNode._parentAddress = _parentAddress;
            await newNode.WriteNodeToDisk().ConfigureAwait(false);
            if (parent.IsFull())
                await parent.SplitNode().ConfigureAwait(false);
        }
                
        private async Task<DataNode<TKey>> BubbleUpToParent(NodeItem<TKey> left, NodeItem<TKey> right)
        {
            if (_parentAddress.HasValue)
            {
                var parent = await _nodeDataSource.Read(_parentAddress.Value).ConfigureAwait(false);
                parent.NodeModified = true;
                
                right.IsLeaf = false;
                left.IsLeaf = false;

                parent._items[left.Key] = left;
                parent._items[right.Key] =right;

                await parent.WriteNodeToDisk().ConfigureAwait(false);

                return parent;
            }
            else
            {
                var newRoot = new DataNode<TKey>(_nodeDataSource, _branchingFactor);
                newRoot.NodeModified = true;
                newRoot._items.Add(left.Key, left);
                newRoot._items.Add(right.Key, right);
                await _nodeDataSource.SetRootNode(newRoot).ConfigureAwait(false);
                return newRoot;
            }
        }       
        
        private async Task<long> WriteNodeToDisk()
        {
            if (NodeModified)
            {
                if (NodeAddress.HasValue)
                    await _nodeDataSource.Update(this).ConfigureAwait(false);
                else
                {
                    NodeAddress = await _nodeDataSource.Add(this).ConfigureAwait(false);
                }
            }
            if (!NodeAddress.HasValue)
                throw new Exception("Node should be written to disk but has no address");

            return NodeAddress.Value;
        }

        private bool IsLeafNode()
        {
            return _items.Values.All(x => x.IsLeaf);
        }

        private bool IsFull()
        {
            return _items.Count() == _branchingFactor;
        }

        private NodeItem<TKey> GetRelevantItem(TKey key)
        {
            NodeItem<TKey> relevantItem = null;
            if (_items.Count == 1)
                relevantItem = _items.Values.First();
            else if (key.CompareTo(_items.Skip(1).Min(x => x.Key)) < 0)
                relevantItem = _items.Values.FirstOrDefault();
            else
                relevantItem = _items.Values.LastOrDefault(x => key.CompareTo(x.Key) >= 0);

            if (relevantItem == null) throw new Exception($"there is no child node for key {key}");
            if (relevantItem.IsLeaf && relevantItem.Value == 0) throw new Exception($"Invalid value for leaf node - {key}");
            if (!relevantItem.IsLeaf && !relevantItem.DataAddress.HasValue) throw new Exception($"there is no data address when it is not a leaf - key - {key}");

            return relevantItem;            
        }

        public void Dispose()
        {
            _nodeDataSource.Dispose();
        }

        public long? NodeAddress { get; set; }
        public bool NodeModified { get; set; }
    }
}
