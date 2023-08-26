using Antlr4.Runtime.Misc;
using Doobee.Parser.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Visitors
{
    internal class TypeVisitor : DoobeeSqlParserBaseVisitor<TypeExpression>
    {
        public override TypeExpression VisitType_name([NotNull] DoobeeSqlParser.Type_nameContext context)
        {
            if (context.INT() != null)
                return new IntTypeExpression();
            else if(context.BOOL() != null) 
                return new BoolTypeExpression();
            else if(context.TEXT() != null) 
                return new TextTypeExpression();

            throw new InvalidDataException("When visiting TypeName for DoobeeParser, there is no valid type specified");
        }
    }
}
