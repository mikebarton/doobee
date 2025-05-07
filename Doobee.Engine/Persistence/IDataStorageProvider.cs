
using Doobee.Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doobee.Persistence
{
    internal interface IDataStorageProvider
    {
        IDataStorage GetItemStorage(Guid id);
    }
}
