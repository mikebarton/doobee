using Doobee.Engine.Storage;
using Doobee.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Schema
{
    internal class SchemaDef 
    {        
        public readonly Dictionary<string, TableDef> _tables = new Dictionary<string, TableDef>();
        
        public TableDef? GetTable(string name)
        {
            return _tables.TryGetValue(name, out TableDef? table) ? table : null;
        }

        public TableDef AddTable(string name)
        {
            var table = new TableDef { Name = name, Id = Guid.NewGuid() };
            _tables.Add(name, table);
            return table;
        }        
    }
}
