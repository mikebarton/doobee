using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Expressions
{
    public abstract class TypeExpression
    {}

    public class BoolTypeExpression : TypeExpression
    {}

    public class TextTypeExpression : TypeExpression { }

    public class IntTypeExpression : TypeExpression
    {}
}
