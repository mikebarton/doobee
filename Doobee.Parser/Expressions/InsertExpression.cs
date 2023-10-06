using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    public class InsertExpression : ParseExpression
    {
        public IdExpression TableName { get; set; }
        public List<IdExpression> ColumnNames { get; set;} = new List<IdExpression>();
        public List<ValuesListExpression> ValuesExpressions { get; set; } = new List<ValuesListExpression>();

    }
}
