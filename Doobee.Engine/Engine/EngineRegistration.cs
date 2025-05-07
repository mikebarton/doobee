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
using Doobee.Engine.Listeners;
using Doobee.Engine.Schema;
using Doobee.Engine.Storage;
using Doobee.Persistence;

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
                services.AddHostedService<Server>();
                services.AddSingleton<EngineConnectionDispatcher>();
                services.AddSingleton<EntityPersistence>(provider =>
                {
                    var storageProvider = provider.GetRequiredService<IDataStorageProvider>();
                    var config = provider.GetRequiredService<DatabaseConfiguration>();
                    var persistence = new EntityPersistence(storageProvider);
                    persistence.Initialise(config).GetAwaiter().GetResult();
                    return persistence;
                });
                services.AddSingleton<SchemaManager>(provider =>
                {
                    var entityPersistence = provider.GetRequiredService<EntityPersistence>();
                    var schemaStorage = entityPersistence
                        .GetOrAddStorage(DatabaseEntities.DatabaseEntity.DatabasesEntityType.Schema).GetAwaiter()
                        .GetResult();
                    var schemaManager = new SchemaManager(schemaStorage);
                    schemaManager.Load().GetAwaiter().GetResult();
                    return schemaManager;
                });
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
