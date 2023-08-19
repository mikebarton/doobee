
using Doobee.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doobee.Engine.Index
{
    internal class NodeDataContext : INodeDataContext
    {
        private IDataStorage _storage;
        private RootItem _rootItem;
        private int _branchingFactor;
        private Dictionary<long, DataNode> _cachedNodes;

        public NodeDataContext(IDataStorage storage, int branchingFactor)
        {
            _storage = storage;
            _rootItem = new RootItem();
            _cachedNodes = new Dictionary<long, DataNode>();
            _branchingFactor = branchingFactor;
            if (_storage.EndOfFileAddress > sizeof(long))
            {
                var rootBytes = _storage.Read(0, sizeof(long));
                var rootAddress = BitConverter.ToInt64(rootBytes, 0);
                _rootItem.RootAddress = rootAddress;
            }
            else
            {
                if (!_rootItem.RootAddress.HasValue)
                    _rootItem.RootAddress = sizeof(long);

                var rootBytes = BitConverter.GetBytes(_rootItem.RootAddress.Value);
                _storage.Write(0, rootBytes);
            }
        }

        public DataNode ReadRootNode()
        {
            if (!_rootItem.RootAddress.HasValue || _rootItem.RootAddress.Value == 0)
                throw new Exception("Root Address has not been set");

            if (!HasRootNode())
            {
                var newRoot = new DataNode(this, _branchingFactor);
                newRoot.NodeAddress = _rootItem.RootAddress;
                WriteNode(newRoot);
                return newRoot;
            }

            return Read(_rootItem.RootAddress.Value);
        }

        public void SetRootNode(DataNode node)
        {
            WriteNode(node);
            _rootItem.RootAddress = node.NodeAddress;
            _storage.Write(0, BitConverter.GetBytes(_rootItem.RootAddress.Value));
        }

        public long Add(DataNode node)
        {
            WriteNode(node);
            return node.NodeAddress.Value;
        }

        public DataNode Read(long address)
        {
            return GetNode(address);
        }

        public void Update(DataNode node)
        {
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
                var newNode = new DataNode(this, _branchingFactor);
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
