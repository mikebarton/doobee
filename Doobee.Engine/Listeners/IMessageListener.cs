using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Listeners
{
    internal interface IMessageListener
    {
        Task Start(Func<DbConnection, Task> messageHandler);
    }
}
