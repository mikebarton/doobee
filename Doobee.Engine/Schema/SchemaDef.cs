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
        private readonly IDataStorage _storage;
        private readonly List<TableDef> _tables = new List<TableDef>();

        public SchemaDef(IDataStorage storage)
        {
            _storage = storage;
        }

        public TableDef AddTable(string name)
        {
            var table = new TableDef(name);
            _tables.Add(table);
            return table;
        }        
    }
}
