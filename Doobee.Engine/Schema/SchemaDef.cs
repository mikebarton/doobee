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
        private readonly List<TableDef> _tables = new List<TableDef>();
            

        public TableDef AddTable(string name)
        {
            var table = new TableDef(name);
            _tables.Add(table);
            return table;
        }        
    }
}
