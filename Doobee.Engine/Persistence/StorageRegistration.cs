using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Doobee.Persistence;

namespace Doobee.Engine.Persistence
{
    internal static class StorageRegistration
    {
        public static IHostBuilder UseFileStorage(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IDataStorageProvider, FileStorageProvider>();
            });

            return builder;
        }

        public static IHostBuilder UseMemoryStorage(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IDataStorageProvider, MemoryStorageProvider>();
            });

            return builder;
        }
    }
}
