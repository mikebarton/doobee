using Doobee.Engine.Engine.Processing;
using Doobee.Engine.Engine.Processing.CreateTable;
using Doobee.Engine.Schema;
using Doobee.Engine.Storage;
using Doobee.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine
{
    internal class StatementDispatcher
    {
        private readonly IDataStorageProvider _storageProvider;
        private readonly ProcessorBase[] _processors;
        private JsonDataRepo? _entitiesStorage;
        private DatabaseEntities? _entities;
        private SchemaManager? _schemaManager;
        

        public StatementDispatcher(IDataStorageProvider storageProvider, ProcessorBase[] processors)
        {
            _storageProvider = storageProvider;            
            _processors = processors;
        }

        public async Task Initialise(DatabaseConfiguration config)
        {
            _entitiesStorage = new JsonDataRepo(_storageProvider.GetItemStorage(config.EngineId));
            _entities = await _entitiesStorage.Read<DatabaseEntities>();
            var schemaId = _entities.Entities.SingleOrDefault(x => x.Type == DatabaseEntities.DatabaseEntity.DatabasesEntityType.Schema);
            if(schemaId == null)
            {
                schemaId = new DatabaseEntities.DatabaseEntity()
                {
                    Id = Guid.NewGuid(),
                    Type = DatabaseEntities.DatabaseEntity.DatabasesEntityType.Schema
                };
                await _entitiesStorage.Write(schemaId);                
            }
            _schemaManager = new SchemaManager(_storageProvider.GetItemStorage(schemaId.Id));
        }

        public async Task<List<Response>> ProcessStatements(IReadOnlyList<Statement> statements)
        {
            if (_schemaManager == null)
                throw new Exception("You must initialise the StatementProcessor before it can process any statements");

            var responses = statements.Select(async x =>
            {
                var processor = _processors.Single(y => y.CanProcess(x));
                var response = await processor.Process(x, _schemaManager);
                return response;
            });

            await Task.WhenAll(responses);

            return responses.Select(x=>x.Result).ToList();
        }
    }
}
