using Doobee.Engine.Engine.Processing;
using Doobee.Engine.Listeners;
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
        private IMessageListener _listener;
        private readonly InstructionsBuilder _instructionsBuilder;
        private readonly DatabaseConfiguration _databaseConfiguration;
        private readonly StatementDispatcher _statementProcessor;
        

        public Engine(DatabaseConfiguration config, IMessageListener listener, InstructionsBuilder instructionsBuilder, StatementDispatcher statementProcessor) 
        {
            _databaseConfiguration = config;
            _listener = listener;
            _instructionsBuilder = instructionsBuilder;
            _statementProcessor = statementProcessor;
        }

        public async Task Start()
        {
            await _statementProcessor.Initialise(_databaseConfiguration);
            await _listener.Start(async (statements) =>
            {
                var statementItems = _instructionsBuilder.Build(statements);
                var responses = await _statementProcessor.ProcessStatements(statementItems);
                //serialize responses
                //return results
                return "result data";
            });
        }
    }
}
