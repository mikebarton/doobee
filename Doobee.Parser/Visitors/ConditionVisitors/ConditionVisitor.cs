using System.Linq.Expressions;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;

namespace Doobee.Parser.Visitors;

public class ConditionVisitor : DoobeeSqlParserBaseVisitor<ConditionExpression>
{
    public override ConditionExpression VisitCondition(DoobeeSqlParser.ConditionContext context)
    {
        var simpleCondition = context.simpleCondition()?.Accept(new SimpleConditionVisitor());
        if(simpleCondition == null)
            throw new SqlParseException("Condition is empty");
        
        var condition = context.condition()?.Accept(new ConditionVisitor());
        
        if (context.AND() != null)
        {
            return new AndConditionExpression()
            {
                LeftExpression = condition,
                RightExpression = simpleCondition
            };
        }
        if (context.OR() != null)
        {
            return new OrConditionExpression()
            {
                LeftExpression = condition,
                RightExpression = simpleCondition
            };
        }

        return simpleCondition;
    }
    
    public override ConditionExpression VisitErrorNode([NotNull] IErrorNode node)
    {
        throw new SqlParseException("unable to parse insert statement");
    }
}