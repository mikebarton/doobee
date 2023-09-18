using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Index
{
    internal interface INodeDataContext<TKey> : IDisposable where TKey : IComparable
    {
        Task<DataNode<TKey>> Read(long address);
        Task<long> Add(DataNode<TKey> node);
        Task Update(DataNode<TKey> node);
        Task Flush();
        Task<DataNode<TKey>> ReadRootNode();
        Task SetRootNode(DataNode<TKey> node);
    }
}
