using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Schema
{
    internal class TableDef
    {
        private readonly List<ColumnDef> _columns = new List<ColumnDef>();

        public TableDef(string name)
        {
            Name = name;
        }

        public ColumnDef AddColumn(string name)
        {
            var col = new ColumnDef(name);
            _columns.Add(col);
            return col;
        }

        public string Name { get; init; }
    }
}
