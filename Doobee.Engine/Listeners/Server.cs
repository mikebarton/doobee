using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Doobee.Engine.Engine;
using Doobee.Engine.Engine.Processing;
using Microsoft.Extensions.Hosting;

namespace Doobee.Engine.Listeners;

internal class Server : IHostedService
{        
    private readonly IMessageListener _listener;
    private readonly DatabaseConfiguration _databaseConfiguration;
    private readonly EngineConnectionDispatcher _engineConnectionDispatcher;

    public Server(DatabaseConfiguration config, IMessageListener listener, EngineConnectionDispatcher dispatcher) 
    {
        _databaseConfiguration = config;
        _listener = listener;
        _engineConnectionDispatcher = dispatcher;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _listener.Start(connection => _engineConnectionDispatcher.Dispatch(connection));
    }

    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    
}