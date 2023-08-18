using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doobee.Storage
{
    internal abstract class DataStorageBase : IDataStorage
    {
        public long Write(long? address, byte[] data)
        {
            long result = default(long);
            if (address.HasValue)
            {
                Storage.Position = address.Value;
                result = address.Value;
            }
            else
            {
                Storage.Position = Storage.Length;
                result = Storage.Length;
            }
            Storage.Write(data, 0, data.Length);
            return result;
        }

        public byte[] Read(long address, int count)
        {
            Storage.Seek(address, SeekOrigin.Begin);            
            var buffer = new byte[count];
            Storage.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public virtual void Flush()
        {
            Storage.Flush();
        }

        public long EndOfFileAddress
        {
            get { return Storage.Length; }
        }

        public abstract Stream Storage { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Storage.Flush();
                Storage.Close();
                Storage.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
