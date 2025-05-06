using System;
using System.Threading.Channels;
using Doobee.Engine.Schema;

namespace Doobee.Engine.Engine;

internal class Table
{
    private readonly TableDef _schema;
    private Channel<Action> _readChannel;
    private Channel<Action> _writeChannel;

    public Table(TableDef schema)
    {
        _schema = schema;
        _readChannel = Channel.CreateUnbounded<Action>(new UnboundedChannelOptions()
        {
            SingleReader = false,
            SingleWriter = false,
        });
        _writeChannel = Channel.CreateUnbounded<Action>(new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = false
        });
    }
}