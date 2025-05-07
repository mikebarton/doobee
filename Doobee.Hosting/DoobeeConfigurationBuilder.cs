using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doobee.Engine.Storage;
using Doobee.Engine.Listeners;
using Doobee.Engine.Engine;
using Doobee.Engine.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Doobee.Engine.Configuration
{
    public class DoobeeConfiguration
    {        
        private HostBuilder _builder;
        private bool _storageConfigured;
        private bool _listenerConfigured;
        private Guid? _id;
        private string? _fileStorageRootPath;

        public DoobeeConfiguration()
        {
            _builder = new HostBuilder();
            _builder.UseEngine();
        }

        public DoobeeConfiguration UseFileStorage(string storageRoot)
        {
            if (_storageConfigured)
                throw new Exception("Storage is already configured");
            
            _fileStorageRootPath = storageRoot;
            _builder.UseFileStorage();
            _storageConfigured = true;
            return this;
        }

        public DoobeeConfiguration UseMemoryStorage()
        {
            if (_storageConfigured)
                throw new Exception("Storage is already configured");

            _builder.UseMemoryStorage();
            _storageConfigured = true;
            return this;
        }

        public DoobeeConfiguration UseTestListener()
        {
            if (_listenerConfigured) 
                throw new Exception("Listener is already configured");

            _builder.UseTestListener();
            _listenerConfigured = true;
            return this;
        }

        public DoobeeConfiguration UseEngineId(Guid? id = null)
        {
            _id = id ?? Guid.NewGuid();
            return this;
        }

        public async Task Start()
        {
            if(_id == null || _id == Guid.Empty)
                _id = Guid.NewGuid();

            _builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton(new DatabaseConfiguration() 
                { 
                    EngineId = _id.Value,
                    FileStorageRootPath = _fileStorageRootPath
                });
            });

            if(!_storageConfigured)
                UseMemoryStorage();

            if(!_listenerConfigured) 
                UseTestListener();

            var host = _builder.Build();
            host.Run();
        }

        public static DoobeeConfiguration Create() => new DoobeeConfiguration();
    }
}
