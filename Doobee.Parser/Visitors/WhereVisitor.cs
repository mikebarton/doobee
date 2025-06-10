using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;

namespace Doobee.Parser.Visitors;

public class WhereVisitor : DoobeeSqlParserBaseVisitor<WhereExpression>
{
    public override WhereExpression VisitWhere_clause(DoobeeSqlParser.Where_clauseContext context)
    {
        return new WhereExpression()
        {
            StartCondition = context.condition().Accept(new ConditionVisitor())
        };
    }
    
    public override WhereExpression VisitErrorNode([NotNull] IErrorNode node)
    {
        throw new SqlParseException("unable to parse insert statement");
    }
}