using Doobee.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Storage
{
    internal static class StorageRegistration
    {
        public static IHostBuilder UseFileStorage(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IDataStorageProvider, FileStorageProvider>();
            });

            return builder;
        }

        public static IHostBuilder UseMemoryStorage(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IDataStorageProvider, MemoryStorageProvider>();
            });

            return builder;
        }
    }
}
