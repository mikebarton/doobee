using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    public class ColumnDefsExpression
    {
        public ColumnDefsExpression(List<ColumnDefExpression> cols) 
        { 
            ColumnDefs = cols;
        }

        public List<ColumnDefExpression> ColumnDefs { get; } = new List<ColumnDefExpression>();
    }
}
