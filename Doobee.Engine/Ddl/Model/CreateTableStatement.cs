using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Ddl.Model
{
    internal class CreateTableStatement
    {
        public CreateTableStatement(string name, List<ColumnDef> columnDefs)
        {
            Name = name;
            ColumnDefs = columnDefs;
        }
        public string Name { get; set; }
        public List<ColumnDef> ColumnDefs { get; set; }

        public enum ColumnType
        {
            Number,
            Text,
            Bool
        }
        public record ColumnDef(string Name, bool Nullable, bool PrimaryKey, ColumnType dataType);
    }
}
