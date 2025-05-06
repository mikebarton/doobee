using System.Threading;
using System.Threading.Tasks;
using Doobee.Engine.Engine;
using Doobee.Engine.Listeners;
using NUnit.Framework;
using Shouldly;

namespace Doobee.Engine.Test.Engine;

[TestFixture]
public class EngineConnectionDispatcherTest
{
    [Test]
    public async Task WhenConnectionDispatched_SubsequentConnectionsWait()
    {
        var conn1 = new DbConnection();
        var conn2 = new DbConnection();
        var sut = new EngineConnectionDispatcher();
        await sut.Dispatch(conn1);
        var secondTask = sut.Dispatch(conn2);
        
        await Task.Delay(200);
        secondTask.IsCompleted.ShouldBeFalse();
        var retrieved1 = await sut.Retrieve(CancellationToken.None);
        retrieved1.ShouldBe(conn1);
        secondTask.IsCompleted.ShouldBeTrue();
    }

    [Test]
    public async Task WhenReadBeforeDispatch_ReadWaits()
    {
        var sut = new EngineConnectionDispatcher();
        var readTask = sut.Retrieve(CancellationToken.None);
        await Task.Delay(200);
        readTask.IsCompleted.ShouldBeFalse();
        
        var conn = new DbConnection();
        await sut.Dispatch(conn);
        readTask.IsCompleted.ShouldBeTrue();
        readTask.Result.ShouldBe(conn);
    }
}