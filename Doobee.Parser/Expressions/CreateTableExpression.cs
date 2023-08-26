using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    public class CreateTableExpression : ParseExpression
    {
        
        public CreateTableExpression(IdExpression id, List<ColumnDefExpression> columns) 
        {
            TableName = id;
            ColumnDefs = columns;
        }

        public IdExpression TableName { get; set; }
        public List<ColumnDefExpression> ColumnDefs { get; set; }
    }
}
