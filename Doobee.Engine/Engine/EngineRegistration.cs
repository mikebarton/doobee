using Doobee.Engine.Engine.Processing;
using Doobee.Engine.Engine.Processing.CreateTable;
using Doobee.Parser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine
{
    internal static class EngineRegistration
    {
        public static IHostBuilder UseEngine(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddTransient<StatementDispatcher>();
                services.AddTransient<InstructionsBuilder>();
                services.AddTransient<SqlParser>();
                services.AddHostedService<Engine>();
            })
            .AddCreateTable();

            return hostBuilder;
        }

        public static IHostBuilder AddCreateTable(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddTransient<ProcessorBase, CreateTableProcessor>();
            });
            return hostBuilder; 
        }

    }
}
