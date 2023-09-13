using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Listeners
{
    internal static class ListenerRegistration
    {
        public static IHostBuilder UseTestListener(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IMessageListener, TestListener>();
            });

            return hostBuilder;
        }
    }
}
