using Doobee.Engine.Messages.Statements;
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
        private readonly DdlProcessor _dlProcessor;

        public StatementProcessor(IDataStorageProvider storageProvider, DdlProcessor ddlProcessor)
        {
            _storageProvider = storageProvider;
            _dlProcessor = ddlProcessor;
        }

        public async Task Initialise(DatabaseConfiguration config)
        {
            _entitiesStorage = new JsonDataRepo(_storageProvider.GetItemStorage(config.EngineId));
            _entities = await _entitiesStorage.Read<DatabaseEntities>();
        }

        public async Task ProcessStatements(IReadOnlyList<CreateTableStatement> statements)
        {
            foreach (var statement in statements)
            {
                if(statement.IsDdl())
                    await _dlProcessor.Process(statement);
            }
        }
    }
}
