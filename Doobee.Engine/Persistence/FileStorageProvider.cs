using Doobee.Engine.Configuration;
using Doobee.Engine.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doobee.Persistence
{
    internal class FileStorageProvider : IDataStorageProvider
    {
        private string _baseFolder;
        private FileStorage? _storage;
        private readonly object _storageLock = new object();
        public FileStorageProvider(DatabaseConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.FileStorageRootPath))
                throw new Exception($"No Root path configured for file storage");

            if(!Directory.Exists(config.FileStorageRootPath))
                Directory.CreateDirectory(config.FileStorageRootPath);
                
            _baseFolder = config.FileStorageRootPath;   
        }
        public IDataStorage GetItemStorage(Guid id)
        {
            var fullPath = Path.Combine(_baseFolder, $"{id.ToString()}.jdb");
            //if (!File.Exists(fullPath))
            //    throw new InvalidOperationException("can not find file " + fullPath);

            if (_storage == null)
            {
                lock (_storageLock)
                {
                    if (_storage == null)
                    {
                        _storage = new FileStorage(fullPath);
                    }
                }
            }
            return _storage;
        }
    }
}
