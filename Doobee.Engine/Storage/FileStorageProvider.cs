using Doobee.Engine.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doobee.Storage
{
    internal class FileStorageProvider : IDataStorageProvider
    {
        public IDataStorage GetItemStorage(Guid id, ConfigurationContext context)
        {
            var fullPath = Path.Combine(context.BaseFolder, $"{id.ToString()}.jdb");
            //if (!File.Exists(fullPath))
            //    throw new InvalidOperationException("can not find file " + fullPath);

            return new FileStorage(fullPath);
        }
    }
}
