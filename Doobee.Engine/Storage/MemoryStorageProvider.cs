using Doobee.Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doobee.Storage
{
    internal class MemoryStorageProvider : IDataStorageProvider
    {
        public IDataStorage GetItemStorage(Guid id)
        {
            return new MemoryStorage();
        }
    }
}
