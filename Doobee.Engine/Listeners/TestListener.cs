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

            await Task.Delay(1000);

            var createFoo = @"create table foo(
                                FooId int primary key,
                                Message text,
                                IsDeleted bool not null
                            );
                            insert into foo 
                                (FooId, Message, IsDeleted) 
                                values (1, 'Hello', false);";
            var connection = new DbConnection() { SqlStatements = createFoo.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) };
            if (_messageHandler != null)
            {
                await _messageHandler(connection);
            }
        }
    }
}
