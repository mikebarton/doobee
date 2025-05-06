using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Doobee.Engine.Listeners;

namespace Doobee.Engine.Engine;

public class EngineConnectionDispatcher
{
    private readonly Channel<DbConnection> _channel;

    public EngineConnectionDispatcher()
    {
        _channel = Channel.CreateBounded<DbConnection>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.Wait,
        });
    }

    public Task Dispatch(DbConnection connection)
    {
        return _channel.Writer.WriteAsync(connection).AsTask();
    }

    public Task<DbConnection> Retrieve(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAsync(cancellationToken).AsTask();
    }
}