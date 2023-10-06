using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    public class ParseExpression
    {
        public bool IsCreateTableStatement => this is CreateTableExpression;
        public bool IsInsertExpression => this is InsertExpression;
    }
}
