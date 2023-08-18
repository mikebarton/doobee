
using Doobee.Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doobee.Storage
{
    internal interface IDataStorageProvider
    {
        IDataStorage GetItemStorage(Guid id, ConfigurationContext context);
    }
}
