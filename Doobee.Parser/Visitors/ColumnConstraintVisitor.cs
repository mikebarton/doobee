using Antlr4.Runtime.Misc;
using Doobee.Parser.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Visitors
{
    internal class ColumnConstraintVisitor : DoobeeSqlParserBaseVisitor<ColumnConstraintExpression>
    {
        public override ColumnConstraintExpression VisitColumn_constraint([NotNull] DoobeeSqlParser.Column_constraintContext context)
        {            
            if ((context.PRIMARY() != null && context.KEY() == null) || (context.PRIMARY() == null && context.KEY() != null))
                throw new Exception("Invalid declaration of partial primary key");

            var isPk = context.PRIMARY() != null && context.KEY() != null;
            var notNull = context.NOT() != null && context.NULL() != null;

            return new ColumnConstraintExpression(isPk, notNull);
        }
    }
}
