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
    internal class ValueExpressionVisitor : DoobeeSqlParserBaseVisitor<ValuesListExpression>
    {       

        public override ValuesListExpression VisitValue_row([NotNull] DoobeeSqlParser.Value_rowContext context)
        {
            foreach (var item in context.children)
            {
                item.Accept(this);
            }

            var rows = context.value_expr().Select(x =>
            {
                var falseContext = x.literal_value().FALSE();
                if (falseContext != null)
                    return new ValueExpression(false);

                var trueContext = x.literal_value().TRUE();
                if (trueContext != null)
                    return new ValueExpression(true);

                var nullContext = x.literal_value().NULL();
                if (nullContext != null)
                    return new ValueExpression(null);

                var numberContext = x.literal_value().NUMERIC_LITERAL();
                if (numberContext != null)
                    return new ValueExpression(double.Parse(numberContext.GetText()));

                var stringContext = x.literal_value().STRING_LITERAL();
                if (stringContext != null)
                    return new ValueExpression(stringContext.GetText().Replace("'", ""));

                throw new SqlParseException("Unable to parse value expression");
            }).ToList();

            return new ValuesListExpression { Values = rows };
        }

        public override ValuesListExpression VisitErrorNode([NotNull] IErrorNode node)
        {
            throw new SqlParseException("unable to parse values list");
        }
    }
}
