using Doobee.Engine.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doobee.Storage
{
    internal class FileStorageProvider : IDataStorageProvider
    {
        private string _baseFolder;
        public FileStorageProvider(string baseFolder)
        {
            _baseFolder = baseFolder;   
        }
        public IDataStorage GetItemStorage(Guid id)
        {
            var fullPath = Path.Combine(_baseFolder, $"{id.ToString()}.jdb");
            //if (!File.Exists(fullPath))
            //    throw new InvalidOperationException("can not find file " + fullPath);

            return new FileStorage(fullPath);
        }
    }
}
