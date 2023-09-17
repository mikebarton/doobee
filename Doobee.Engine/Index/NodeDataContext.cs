
using Doobee.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        }

        public async Task Initialise()
        {
            if (_storage.EndOfFileAddress > sizeof(long))
            {
                var rootBytes = await _storage.Read(0, sizeof(long));
                var rootAddress = BitConverter.ToInt64(rootBytes, 0);
                _rootItem.RootAddress = rootAddress;
            }
            else
            {
                if (!_rootItem.RootAddress.HasValue)
                    _rootItem.RootAddress = sizeof(long);

                var rootBytes = BitConverter.GetBytes(_rootItem.RootAddress.Value);
                await _storage.Write(0, rootBytes).ConfigureAwait(false);
            }
        }

        public async Task<DataNode> ReadRootNode()
        {
            if (!_rootItem.RootAddress.HasValue || _rootItem.RootAddress.Value == 0)
                throw new Exception("Root Address has not been set");

            if (!HasRootNode())
            {
                var newRoot = new DataNode(this, _branchingFactor);
                newRoot.NodeAddress = _rootItem.RootAddress;
                await WriteNode(newRoot).ConfigureAwait(false);
                return newRoot;
            }

            return await Read(_rootItem.RootAddress.Value).ConfigureAwait(false);
        }

        public async Task SetRootNode(DataNode node)
        {
            await WriteNode(node).ConfigureAwait(false);
            _rootItem.RootAddress = node.NodeAddress;
            await _storage.Write(0, BitConverter.GetBytes(_rootItem.RootAddress.Value)).ConfigureAwait(false);
        }

        public async Task<long> Add(DataNode node)
        {
            await WriteNode(node).ConfigureAwait(false);
            return node.NodeAddress.Value;
        }

        public Task<DataNode> Read(long address)
        {
            return GetNode(address);
        }

        public async Task Update(DataNode node)
        {
            if (node.NodeAddress.HasValue)
            {
                await WriteNode(node).ConfigureAwait(false);
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

        private async Task WriteNode(DataNode node)
        {
            var data = node.ToByteArray();
            if (node.NodeAddress.HasValue)
            {
                await _storage.Write(node.NodeAddress.Value, data).ConfigureAwait(false);
            }
            else
            {
                node.NodeAddress = await _storage.Write(null, data).ConfigureAwait(false);
                _cachedNodes.Add(node.NodeAddress.Value, node);                
            }            
        }

        private async Task<DataNode> GetNode(long address)
        {
            if (_cachedNodes.ContainsKey(address))
                return _cachedNodes[address];
            else
            {
                var newNode = new DataNode(this, _branchingFactor);
                var nodeBytes = await _storage.Read(address, DataNode.GetNodeSize(_branchingFactor)).ConfigureAwait(false);
                newNode.Load(nodeBytes);
                newNode.NodeAddress = address;
                _cachedNodes.Add(address, newNode);
                return newNode;
            }
        }

        public async Task Flush()
        {
            var rootBytes = BitConverter.GetBytes(_rootItem.RootAddress.HasValue ? _rootItem.RootAddress.Value : 0);
            await _storage.Write(0, rootBytes).ConfigureAwait(false);

            foreach (var node in _cachedNodes.Values)
            {
                if (node.NodeModified)
                    await WriteNode(node).ConfigureAwait(false);
            }
            await _storage.Flush().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Flush().GetAwaiter().GetResult();
            _storage.Dispose();
        }
    }
}
