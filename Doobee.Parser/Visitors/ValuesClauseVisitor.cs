using Antlr4.Runtime.Misc;
using Doobee.Parser.Expressions;

namespace Doobee.Parser.Visitors;

public class ValuesClauseVisitor : DoobeeSqlParserBaseVisitor<List<ValuesListExpression>>
{
    public override List<ValuesListExpression> VisitValues_clause([NotNull] DoobeeSqlParser.Values_clauseContext context)
    {
        context.children[0].Accept(this);
        var values = context.value_row().Select(x=>x.Accept(new ValueExpressionVisitor())).ToList();
        return values;
    }
}