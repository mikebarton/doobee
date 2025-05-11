using Doobee.Engine.Configuration;
using Doobee.Engine.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doobee.Persistence
{
    internal class FileStorageProvider : IDataStorageProvider
    {
        private string _baseFolder;

        private readonly ConcurrentDictionary<string, FileStorage> _fileStorage =
            new ConcurrentDictionary<string, FileStorage>();
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
        public IDataStorage GetItemStorage(Guid id, string? variant = null)
        {
            var fullPath = variant == null ? 
                Path.Combine(_baseFolder, $"{id.ToString()}.jdb") : 
                Path.Combine(_baseFolder, $"{id.ToString()}-{variant}.jdb");
            //if (!File.Exists(fullPath))
            //    throw new InvalidOperationException("can not find file " + fullPath);
            
            
            FileStorage? fileStorage = null;
            var cacheKey = GetCacheKey(id, variant);
            if (!_fileStorage.TryGetValue(cacheKey, out fileStorage))
            {
                lock (_storageLock)
                {
                    if (!_fileStorage.TryGetValue(cacheKey, out fileStorage))
                    {
                        fileStorage = new FileStorage(fullPath);
                        _fileStorage[cacheKey] = fileStorage;
                    }
                }
            }
            return fileStorage;
        }
        
        private string GetCacheKey(Guid id, string? variant) => $"{id.ToString()}-{variant ?? "none"}";
    }
}
