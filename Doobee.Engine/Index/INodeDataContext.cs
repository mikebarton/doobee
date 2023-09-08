using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Index
{
    internal interface INodeDataContext : IDisposable
    {
        Task<DataNode> Read(long address);
        Task<long> Add(DataNode node);
        Task Update(DataNode node);
        Task Flush();
        Task<DataNode> ReadRootNode();
        Task SetRootNode(DataNode node);
    }
}
