using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doobee.Storage
{
    internal class FileStorage : DataStorageBase
    {
        public FileStorage(string filePath)
        {
            Storage = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public override void Flush()
        {
            ((FileStream)Storage).Flush(true);
        }

        public override Stream Storage { get; set; }
    }
}
