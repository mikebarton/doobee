﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Storage
{
    internal abstract class DataStorageBase : IDataStorage
    {
        public async Task<long> Write(long? address, byte[] data)
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
            await Storage.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            await Storage.FlushAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<byte[]> Read(long address, long count)
        {
            Storage.Seek(address, SeekOrigin.Begin);            
            var buffer = new byte[count];
            await Storage.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            return buffer;
        }

        public virtual Task Flush()
        {
            return Storage.FlushAsync();
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

        public Task Clear()
        {
            Storage.SetLength(0);
            return Storage.FlushAsync();
        }
    }
}
