using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Persistence
{
    internal class FileStorage : DataStorageBase
    {
        public FileStorage(string filePath)
        {
            Storage = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }        

        public override Stream Storage { get; set; }
    }
}
