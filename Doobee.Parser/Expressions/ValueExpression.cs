using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    public class ValueExpression
    {
        public ValueExpression(dynamic val) 
        {
            Value = val;
        }

        public dynamic Value { get; init; }
    }
}
