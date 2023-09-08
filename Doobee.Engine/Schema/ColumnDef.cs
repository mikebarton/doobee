using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Schema
{
    internal class ColumnDef
    {
        public ColumnDef(string name)
        {
            Name = name;
        }

        public string Name { get; init; }
        public bool Nullable { get; init; }
        public bool PrimaryKey { get; init; }
    }
}
