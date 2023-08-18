
using Doobee.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doobee.Engine.Index
{
    internal class NodeDataContext : INodeDataContext
    {
        private IDataStorage _storage;
        private bool _isInitialised;
        private RootItem _rootItem;
        private int _branchingFactor;
        private Dictionary<long, DataNode> _cachedNodes;

        public NodeDataContext(IDataStorage storage)
        {
            _storage = storage;
        }

        public void Initialise(int branchingFactor, RootItem root)
        {
            if (!_isInitialised)
            {
                _cachedNodes = new Dictionary<long, DataNode>();
                _branchingFactor = branchingFactor;
                _rootItem = root;
                _isInitialised = true;
                if(_storage.EndOfFileAddress > sizeof(long))
                {
                    var rootBytes = _storage.Read(0, sizeof(long));
                    var rootAddress = BitConverter.ToInt64(rootBytes, 0);
                    _rootItem.RootAddress = rootAddress;
                }
                else
                {
                    if (!root.RootAddress.HasValue)
                        root.RootAddress = sizeof(long);

                    var rootBytes = BitConverter.GetBytes(root.RootAddress.Value);
                    _storage.Write(0, rootBytes);
                }
            }
        }

        public void EnsureInitialised()
        {
            if (!_isInitialised)
                throw new InvalidOperationException("Cannot use the NodeDataContext when it has not been intialised");
        }

        public long Add(DataNode node)
        {
            EnsureInitialised();

            WriteNode(node);
            return node.NodeAddress.Value;
        }

        public DataNode Read(long address)
        {
            EnsureInitialised();
            
            return GetNode(address);
        }

        public void Update(DataNode node)
        {
            EnsureInitialised();

            if (node.NodeAddress.HasValue)
            {
                WriteNode(node);
            }
            else
            {
                throw new Exception("can not update node in data storage with no nodeaddress");
            }
        }       
        
        public bool HasRootNode()
        {
            return _storage.EndOfFileAddress > DataNode.GetNodeSize(_branchingFactor);
        }

        private void WriteNode(DataNode node)
        {
            var data = node.ToByteArray();
            if (node.NodeAddress.HasValue)
            {
                _storage.Write(node.NodeAddress.Value, data);
            }
            else
            {
                node.NodeAddress = _storage.Write(null, data);
                _cachedNodes.Add(node.NodeAddress.Value, node);                
            }
        }

        private DataNode GetNode(long address)
        {
            if (_cachedNodes.ContainsKey(address))
                return _cachedNodes[address];
            else
            {
                var newNode = new DataNode(this, _branchingFactor, _rootItem);
                var nodeBytes = _storage.Read(address, DataNode.GetNodeSize(_branchingFactor));
                newNode.Load(nodeBytes);
                newNode.NodeAddress = address;
                _cachedNodes.Add(address, newNode);
                return newNode;
            }
        }

        public void Flush()
        {
            var rootBytes = BitConverter.GetBytes(_rootItem.RootAddress.HasValue ? _rootItem.RootAddress.Value : 0);
            _storage.Write(0, rootBytes);

            foreach (var node in _cachedNodes.Values)
            {
                if(node.NodeModified)
                    WriteNode(node);
            }
            _storage.Flush();
        }

        public void Dispose()
        {
            Flush();
            _storage.Dispose();
        }
    }
}
