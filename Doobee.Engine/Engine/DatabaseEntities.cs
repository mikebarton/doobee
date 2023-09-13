using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine
{
    internal class DatabaseEntities
    {
        public Guid Id { get; init; }

        public List<DatabaseEntity> Entities { get; init; } = new List<DatabaseEntity>();

        internal class DatabaseEntity
        {
            public Guid Id { get; init; }

            public DatabasesEntityType Type { get; init; }

            public enum DatabasesEntityType
            {
                Schema
            }
        }
    }

}
