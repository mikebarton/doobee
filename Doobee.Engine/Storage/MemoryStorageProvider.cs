using Doobee.Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doobee.Storage
{
    internal class MemoryStorageProvider : IDataStorageProvider
    {
        public IDataStorage GetItemStorage(Guid id, ConfigurationContext context)
        {
            return new MemoryStorage();
        }
    }
}
