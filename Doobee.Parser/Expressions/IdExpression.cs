using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    public class IdExpression : ValueExpression
    {
        public IdExpression(string Id) : base(Id)
        {}
        
        public string Id => base.Value.ToString();
    }
}
