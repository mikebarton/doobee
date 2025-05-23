﻿using Doobee.Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doobee.Persistence
{
    internal class MemoryStorageProvider : IDataStorageProvider
    {
        public IDataStorage GetItemStorage(Guid id, string? variant = null)
        {
            return new MemoryStorage();
        }
    }
}
