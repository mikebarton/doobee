﻿using Doobee.Engine.Engine.Processing;
using Doobee.Engine.Listeners;
using Doobee.Engine.Storage;
using Doobee.Persistence;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine
{
    internal class Engine : IHostedService
    {        
        private readonly InstructionsBuilder _instructionsBuilder;
        private readonly StatementDispatcher _statementProcessor;
        private readonly EngineConnectionDispatcher _engineConnectionDispatcher;
        private bool _isRunning;

        public Engine(InstructionsBuilder instructionsBuilder, StatementDispatcher statementProcessor, EngineConnectionDispatcher dispatcher) 
        {
            _instructionsBuilder = instructionsBuilder;
            _statementProcessor = statementProcessor;
            _engineConnectionDispatcher = dispatcher;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;
            DoWork(cancellationToken);
        }

        private async void DoWork(CancellationToken cancellationToken)
        {
            while (_isRunning)
            {
                var connection = await _engineConnectionDispatcher.Retrieve(cancellationToken);
                var statementItems = _instructionsBuilder.Build(connection.SqlStatements);
                var responses = await _statementProcessor.ProcessStatements(statementItems);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isRunning = false;
            return Task.CompletedTask;
        }
    }
}
