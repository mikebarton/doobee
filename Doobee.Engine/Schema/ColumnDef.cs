using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Schema
{
    internal class ColumnDef
    {        
        public IndexDef? Index { get; set; }
        public string Name { get; init; }
        public bool Nullable { get; set; }
        public bool PrimaryKey { get; set; }
    }
}
