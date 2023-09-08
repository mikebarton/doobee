using Doobee.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine
{
    internal class Engine
    {
        private readonly IDataStorageProvider _storageProvider;
        private IDataStorage _storage;
        private readonly DatabaseConfiguration _databaseConfiguration;
        

        public Engine(IDataStorageProvider storageProvider, DatabaseConfiguration config) 
        {
            _storageProvider = storageProvider;
            _databaseConfiguration = config;
        }

        public async Task Start()
        {
            _storage = _storageProvider.GetItemStorage(_databaseConfiguration.EngineId);
            var dbEntitiesBytes = _storage.Read(0, _storage.EndOfFileAddress);
        }
    }
}
