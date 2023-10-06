using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Schema
{
    internal class TableDef
    {
        public readonly List<ColumnDef> _columns = new List<ColumnDef>();
        

        public ColumnDef AddColumn(string name)
        {
            var col = new ColumnDef{ Name = name };
            _columns.Add(col);
            return col;
        }

        public string Name { get; init; }
        public Guid Id { get; init; }
    }
}
