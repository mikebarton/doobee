using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Listeners
{
    internal class TestListener : IMessageListener
    {
        private Func<string[], Task<string>>? _messageHandler;
        public Task Start(Func<string[], Task<string>> messageHandler)
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

            var response = await _messageHandler(new[] { createFoo });
        }
    }
}
