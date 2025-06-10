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
    internal class ValueListExpressionVisitor : DoobeeSqlParserBaseVisitor<ValuesListExpression>
    {       

        public override ValuesListExpression VisitValue_row([NotNull] DoobeeSqlParser.Value_rowContext context)
        {
            foreach (var item in context.children)
            {
                item.Accept(this);
            }

            var rows = context.value_expr()
                .Select(x => x.Accept(new ValueExpressionVisitor()))
                .ToList();

            return new ValuesListExpression { Values = rows };
        }

        public override ValuesListExpression VisitErrorNode([NotNull] IErrorNode node)
        {
            throw new SqlParseException("unable to parse values list");
        }
    }
}
