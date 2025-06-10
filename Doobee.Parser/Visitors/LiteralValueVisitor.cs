using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;

namespace Doobee.Parser.Visitors;

public class LiteralValueVisitor : DoobeeSqlParserBaseVisitor<ValueExpression>
{
    public override ValueExpression VisitLiteral_value(DoobeeSqlParser.Literal_valueContext context)
    {
        var falseContext = context.FALSE();
        if (falseContext != null)
            return new ValueExpression(false);

        var trueContext = context.TRUE();
        if (trueContext != null)
            return new ValueExpression(true);

        var nullContext = context.NULL();
        if (nullContext != null)
            return new ValueExpression(null);

        var numberContext = context.NUMERIC_LITERAL();
        if (numberContext != null)
            return new ValueExpression(double.Parse(numberContext.GetText()));

        var stringContext = context.STRING_LITERAL();
        if (stringContext != null)
            return new ValueExpression(stringContext.GetText().Replace("'", ""));

        throw new SqlParseException("Unable to parse value expression");
    }
    
    public override ValueExpression VisitErrorNode([NotNull] IErrorNode node)
    {
        throw new SqlParseException("unable to parse insert statement");
    }
}