using System;
using System.Collections.Generic;
using System.Text;

namespace Doobee.Storage
{
    internal interface IDataStorage : IDisposable
    {
        long Write(long? address, byte[] data);
        byte[] Read(long address, int count);
        long EndOfFileAddress { get; }
        void Flush();
    }
}
