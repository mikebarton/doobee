using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    internal class ColumnConstraintExpression
    {
        public ColumnConstraintExpression(bool isPrimaryKey, bool notNull) 
        {
            PrimaryKey = isPrimaryKey;
            NotNull = notNull;
        }

        public bool PrimaryKey { get; set; }
        public bool NotNull { get; set; }
    }
}
