using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Doobee.Engine.Index
{
    internal partial class DataNode : IDisposable
    {
        private INodeDataContext _nodeDataSource;
        private SortedList<long, NodeItem> _items;
        private int _branchingFactor;
        private long? _parentAddress;
               

        public DataNode(INodeDataContext nodeDataSource, int branchingFactor)
        {
            _nodeDataSource = nodeDataSource;
            _items = new SortedList<long, NodeItem>();
            _branchingFactor = branchingFactor;
        }        

        public void Insert(long key, long value)
        {
            var root = _nodeDataSource.ReadRootNode();
            root.InsertInternal(key, value);
        }

        public long Query(long key)
        {
            var root = _nodeDataSource.ReadRootNode();
            return root.QueryInternal(key);
        }

        public void Flush()
        {
            _nodeDataSource.Flush();
        }

        private long QueryInternal(long key)
        {
            if (_items.Count == 0)
                return -1;

            var relevantItem = GetRelevantItem(key);
            if (relevantItem == null)
                return -1;
                //throw new Exception($"no node item available for key {key}");

            if (IsLeafNode())
            {
                if (relevantItem.Key == key)
                    return relevantItem.Value;
                else
                    return -1;

            }

            var childNode = _nodeDataSource.Read(relevantItem.DataAddress.Value);
            if (childNode == null)
                throw new Exception($"No child available for key {key} at address {relevantItem.DataAddress}");
            return childNode.QueryInternal(key);
        }

        private void InsertInternal(long key, long value)
        {
            if (!IsLeafNode())
            {
                var item = GetRelevantItem(key);
                var child = _nodeDataSource.Read(item.DataAddress.Value);
                child.InsertInternal(key, value); 
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
                    _items.Add(key, new NodeItem
                    {
                        Key = key,
                        Value = value,
                        IsLeaf = true
                    });

                    if (IsFull())
                    {
                        SplitNode();
                    }
                }
                WriteNodeToDisk();
            }
        }

        private void SplitNode()
        {
            var splitIndex = _items.Count / 2;
            var newNode = new DataNode(_nodeDataSource, _branchingFactor);
            newNode.NodeModified = true;
            var newNodeAddress = newNode.WriteNodeToDisk();
            var rightItems = _items.Skip(splitIndex).ToList();            
            _items = new SortedList<long, NodeItem>(_items.Values.Take(splitIndex).ToDictionary(x => x.Key));
            NodeModified = true;
            newNode.NodeModified = true;
            foreach (var item in rightItems)
            {
                newNode._items.Add(item.Key, item.Value);
                if (!item.Value.IsLeaf)
                {
                    var childNode = _nodeDataSource.Read(item.Value.DataAddress.Value);
                    childNode._parentAddress = newNodeAddress;
                    childNode.NodeModified = true;
                }
            }            

            var newNodeItem = new NodeItem
            {
                Key = rightItems.Min(x => x.Value.Key),
                DataAddress = newNodeAddress,
                IsLeaf = false
            };

            var leftItem = new NodeItem
            {
                Key = _items.Min(x => x.Key),
                DataAddress = NodeAddress,
                IsLeaf = false
            };

            var parent = BubbleUpToParent(leftItem, newNodeItem);
            _parentAddress = parent.NodeAddress;
            newNode._parentAddress = _parentAddress;
            newNode.WriteNodeToDisk();
            if (parent.IsFull())
                parent.SplitNode();
        }
                
        private DataNode BubbleUpToParent(NodeItem left, NodeItem right)
        {
            if (_parentAddress.HasValue)
            {
                var parent = _nodeDataSource.Read(_parentAddress.Value);
                parent.NodeModified = true;
                
                right.IsLeaf = false;
                left.IsLeaf = false;

                parent._items[left.Key] = left;
                parent._items[right.Key] =right;

                parent.WriteNodeToDisk();

                return parent;
            }
            else
            {
                var newRoot = new DataNode(_nodeDataSource, _branchingFactor);
                newRoot.NodeModified = true;
                newRoot._items.Add(left.Key, left);
                newRoot._items.Add(right.Key, right);
                _nodeDataSource.SetRootNode(newRoot);
                return newRoot;
            }
        }       
        
        private long WriteNodeToDisk()
        {
            if (NodeModified)
            {
                if (NodeAddress.HasValue)
                    _nodeDataSource.Update(this);
                else
                {
                    NodeAddress = _nodeDataSource.Add(this);
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

        private NodeItem GetRelevantItem(long key)
        {

            NodeItem relevantItem = null;
            if (_items.Count == 1)
                relevantItem = _items.Values.First();
            else if (key < _items.Skip(1).Min(x => x.Key))
                relevantItem = _items.Values.FirstOrDefault();
            else
                relevantItem = _items.Values.LastOrDefault(x => key >= x.Key);

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
