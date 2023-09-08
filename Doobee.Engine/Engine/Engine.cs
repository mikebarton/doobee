using Doobee.Engine.Storage;
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
        private JsonDataRepo? _entitiesStorage;
        private DatabaseEntities? _entities;
        private readonly DatabaseConfiguration _databaseConfiguration;
        

        public Engine(IDataStorageProvider storageProvider, DatabaseConfiguration config) 
        {
            _storageProvider = storageProvider;
            _databaseConfiguration = config;
        }

        public async Task Start()
        {
            _entitiesStorage = new JsonDataRepo(_storageProvider.GetItemStorage(_databaseConfiguration.EngineId));
            _entities = await _entitiesStorage.Read<DatabaseEntities>();
        }
    }
}
