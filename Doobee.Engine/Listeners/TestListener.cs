using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Listeners
{
    internal class TestListener : IMessageListener
    {
        private Func<DbConnection, Task>? _messageHandler;
        public Task Start(Func<DbConnection, Task> messageHandler)
        {
            _messageHandler = messageHandler;
            _ = DoSomething();
            return Task.CompletedTask;
        }

        public async Task DoSomething()
        {

            await Task.Delay(5000);

            var createFoo = @"create table foo(
                                FooId int primary key,
                                Message text,
                                IsDeleted bool not null
                            )";
            var connection = new DbConnection() { SqlStatements = new[] { createFoo } };
            if (_messageHandler != null)
            {
                await _messageHandler(connection);
            }
        }
    }
}
