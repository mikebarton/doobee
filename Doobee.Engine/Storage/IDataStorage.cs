using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Storage
{
    internal interface IDataStorage : IDisposable
    {
        Task<long> Write(long? address, byte[] data);
        Task<byte[]> Read(long address, long count);
        long EndOfFileAddress { get; }
        Task Flush();
    }
}
