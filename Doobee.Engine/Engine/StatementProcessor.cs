using Doobee.Engine.Messages.Responses;
using Doobee.Engine.Messages.Statements;
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
    internal class StatementProcessor
    {
        private readonly IDataStorageProvider _storageProvider;
        private JsonDataRepo? _entitiesStorage;
        private DatabaseEntities? _entities;
        private SchemaManager? _schemaManager;
        private readonly DdlProcessor _dlProcessor;
        private readonly DmlProcessor _dlmlProcessor;

        public StatementProcessor(IDataStorageProvider storageProvider, DdlProcessor ddlProcessor, DmlProcessor dmlProcessor)
        {
            _storageProvider = storageProvider;
            _dlProcessor = ddlProcessor;
            _dlmlProcessor = dmlProcessor;
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

        public async Task<List<Response>> ProcessStatements(IReadOnlyList<CreateTableStatement> statements)
        {
            if (_schemaManager == null)
                throw new Exception("You must initialise the StatementProcessor before it can process any statements");

            var responses = statements.Select(async x =>
            {

                if (x.IsDdl())
                    return await _dlProcessor.Process(x, _schemaManager);
                else
                    return await _dlmlProcessor.Process(x, _schemaManager);

            });

            await Task.WhenAll(responses);

            return responses.Select(x=>x.Result).ToList();
        }
    }
}
