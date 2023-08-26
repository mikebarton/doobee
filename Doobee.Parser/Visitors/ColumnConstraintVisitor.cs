using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
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
        public override ColumnConstraintExpression VisitColumn_constraint([NotNull] Parser.Column_constraintContext context)
        {            
            var primaryContext = context.PRIMARY();
            var keyContext = context.KEY();
            var notContext = context.NOT();
            var nullContext = context.NULL();

            var isValid = (ITerminalNode? node) => node != null && node.Symbol.TokenIndex > -1;

            if((isValid(primaryContext) && !isValid(keyContext)) || (!isValid(primaryContext) && isValid(keyContext)))
                throw new Exception("Invalid declaration of partial primary key");


            var isPk = isValid(primaryContext) && isValid(keyContext);
            var notNull = isValid(notContext) && isValid(nullContext);

            return new ColumnConstraintExpression(isPk, notNull);
        }
    }
}
