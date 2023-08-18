using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doobee.Engine.Index
{
    internal partial class DataNode : IDisposable
    {
        private INodeDataContext _nodeDataSource;
        private SortedList<long, NodeItem> _items;
        private NodeItem _minItem;
        private int _branchingFactor;
        private RootItem _rootItem;
        private long? _parentAddress;
               

        public DataNode(INodeDataContext nodeDataSource, int branchingFactor)
        {
            _nodeDataSource = nodeDataSource;
            _items = new SortedList<long, NodeItem>();
            _branchingFactor = branchingFactor;
            _rootItem = new RootItem();
            _nodeDataSource.Initialise(branchingFactor, _rootItem);
        }

        internal DataNode(INodeDataContext nodeDataSource, int branchingFactor, RootItem root)
            : this(nodeDataSource, branchingFactor)
        {
            _rootItem = root;
        }

        public void Insert(long key, long address)
        {
            if (!_rootItem.RootAddress.HasValue)
            {
                var newRoot = new DataNode(_nodeDataSource, _branchingFactor, _rootItem);
                newRoot.NodeModified = true;
                _rootItem.RootAddress = newRoot.WriteNodeToDisk();
            }

            var root = _nodeDataSource.Read(_rootItem.RootAddress.Value);
            root.InsertInternal(key, address);
        }

        public long Query(long key)
        {
            if (!_rootItem.RootAddress.HasValue)
            {
                return -1;
            }

            var root = _nodeDataSource.Read(_rootItem.RootAddress.Value);
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

            var relevantItem = key < _items.Keys.Min() ?
                                _minItem :
                                _items.Values.LastOrDefault(x => key >= x.Key);
            if (relevantItem == null)
                return -1;
                //throw new Exception($"no node item available for key {key}");

            if (IsLeafNode())
            {
                if (relevantItem.Key == key)
                    return relevantItem.DataAddress.Value;
                else
                    return -1;

            }

            var childNode = _nodeDataSource.Read(relevantItem.DataAddress.Value);
            if (childNode == null)
                throw new Exception($"No child available for key {key} at address {relevantItem.DataAddress}");
            return childNode.QueryInternal(key);
        }

        private void InsertInternal(long key, long address)
        {
            if (!IsLeafNode())
            {
                var child = GetRelevantChildNode(key);
                child.InsertInternal(key, address); 
            }
            else
            {
                NodeModified = true;
                if (_items.ContainsKey(key))
                {
                    _items[key].DataAddress = address;
                }
                else
                {
                    _items.Add(key, new NodeItem
                    {
                        Key = key,
                        DataAddress = address,
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
            var newNode = new DataNode(_nodeDataSource, _branchingFactor, _rootItem);
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

                if(parent._minItem != null && left.Key <= parent._minItem.Key)
                {
                    parent._minItem = left;
                }
                parent._items.Add(right.Key, right);



                return parent;
            }
            else
            {
                var newRoot = new DataNode(_nodeDataSource, _branchingFactor, _rootItem);
                newRoot.NodeModified = true;
                _rootItem.RootAddress = newRoot.WriteNodeToDisk();
                newRoot._minItem = left;
                newRoot._items.Add(right.Key, right);
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

        private DataNode GetRelevantChildNode(long key)
        {
            NodeItem relevantItem = null;
            if (key < _items.Min(x => x.Key))
                relevantItem = _minItem;
            else
                relevantItem = _items.Values.LastOrDefault(x => key >= x.Key);

            if (relevantItem == null) throw new Exception($"there is no child node for key {key}");
            if (relevantItem.IsLeaf) throw new Exception($"can not get child node when item is a leaf - key - {key}");
            if (!relevantItem.DataAddress.HasValue) throw new Exception($"there is no data address when it is not a leaf - key - {key}");

            var node = _nodeDataSource.Read(relevantItem.DataAddress.Value);
            return node;
        }

        public void Dispose()
        {
            _nodeDataSource.Dispose();
        }

        public long? NodeAddress { get; set; }
        public bool NodeModified { get; set; }
    }
}
