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
            return Task.CompletedTask;
        }
    }
}
