using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;

namespace Doobee.Parser.Visitors;

public class ValueExpressionVisitor : DoobeeSqlParserBaseVisitor<ValueExpression>
{
    public override ValueExpression VisitValue_expr(DoobeeSqlParser.Value_exprContext context)
    {
        return context.literal_value().Accept(new LiteralValueVisitor());
    }
    
    public override ValueExpression VisitErrorNode([NotNull] IErrorNode node)
    {
        throw new SqlParseException("unable to parse insert statement");
    }
}