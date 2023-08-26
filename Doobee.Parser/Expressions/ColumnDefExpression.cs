using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    internal class ColumnDefExpression
    {
        public ColumnDefExpression(IdExpression columnName, TypeExpression type, List<ColumnConstraintExpression> constraints)
        {
            ColumnName = columnName;
            ColumnType = type;
            Constraints = constraints;
        }

        public IdExpression ColumnName { get; set; }
        public TypeExpression ColumnType { get; set; }
        public List<ColumnConstraintExpression> Constraints { get; set; }
    }
}
