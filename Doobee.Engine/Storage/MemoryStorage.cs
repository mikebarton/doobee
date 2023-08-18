using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doobee.Storage
{
    internal class MemoryStorage : DataStorageBase
    {
        public MemoryStorage()
        {
            Storage = new MemoryStream();
        }

        public override Stream Storage { get; set; }
    }
}
