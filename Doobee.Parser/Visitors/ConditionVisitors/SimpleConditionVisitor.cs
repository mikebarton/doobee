using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;

namespace Doobee.Parser.Visitors;

public class SimpleConditionVisitor : DoobeeSqlParserBaseVisitor<ConditionExpression>
{
    public override ConditionExpression VisitEqualityCondition(DoobeeSqlParser.EqualityConditionContext context)
    {
        return new EqualExpression()
        {
            LeftExpression = context.column_name().Accept(new IDVisitor()),
            RightExpression = context.literal_value().Accept(new ValueExpressionVisitor())
        };
    }

    public override ConditionExpression VisitInCondition(DoobeeSqlParser.InConditionContext context)
    {
        return new InRangeExpression()
        {
            ColumnName = context.column_name().Accept(new IDVisitor()),
            Values = context
                .literal_value()
                .Select(x => x.Accept(new LiteralValueVisitor()))
                .ToList()
        };
    }

    public override ConditionExpression VisitLikeCondition(DoobeeSqlParser.LikeConditionContext context)
    {
        return new LikeExpression()
        {
            ColumnName = context.column_name().Accept(new IDVisitor()),
            Value = context.literal_value().Accept(new LiteralValueVisitor())
        };
    }

    public override ConditionExpression VisitRangeCondition(DoobeeSqlParser.RangeConditionContext context)
    {
        var literalValueContexts = context.literal_value();
        if(literalValueContexts.Length != 2)
            throw new SqlParseException("Invalid number of literals for range condition");

        return new BetweenExpression()
        {
            ColumnName = context.column_name().Accept(new IDVisitor()),
            LeftExpression = literalValueContexts[0].Accept(new LiteralValueVisitor()),
            RightExpression = literalValueContexts[1].Accept(new LiteralValueVisitor())
        };
    }

    public override ConditionExpression VisitIsNullCondition(DoobeeSqlParser.IsNullConditionContext context)
    {
        var nullCondition = new IsNullCondition()
        {
            ColumnExpression = context.column_name().Accept(new IDVisitor()),
        };
        
        if(context.NOT() == null)
            return nullCondition;

        return new NotConditionExpression()
        {
            Condition = nullCondition
        };
    }

    public override ConditionExpression VisitSubqueryCondition(DoobeeSqlParser.SubqueryConditionContext context)
    {
        if (context.EQUALS() != null)
        {
            return new SubqueryEqualsExpression()
            {
                ColumnExpression = context.column_name().Accept(new IDVisitor()),
                SelectExpression = context.select_stmt().Accept(new SelectVisitor())
            };
        }
        
        return new SubqueryInExpression()
        {
            ColumnExpression = context.column_name().Accept(new IDVisitor()),
            SelectExpression = context.select_stmt().Accept(new SelectVisitor())
        };
    }
    
    public override ConditionExpression VisitErrorNode([NotNull] IErrorNode node)
    {
        throw new SqlParseException("unable to parse insert statement");
    }
}