using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    internal abstract class TypeExpression
    {
        public TypeExpression()
        {
            
        }
        public string TypeName { get; set; }
        public TypeEnum TypeInstance { get; set; }
        public enum TypeEnum
        {
            Int,
            Bool,
            Text
        }
    }

    internal class BoolTypeExpression : TypeExpression
    {
        public BoolTypeExpression()
        {}
    }

    internal class TextTypeExpression : TypeExpression { }

    internal class IntTypeExpression : TypeExpression
    {
        public IntTypeExpression()
        {}
    }
}
