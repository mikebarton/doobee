namespace Doobee.Parser.Expressions;

public class ConditionExpression
{
}

public class IsNullCondition : ConditionExpression
{
    public IdExpression ColumnExpression { get; set; } = null!;
}

public class NotConditionExpression : ConditionExpression
{
    public ConditionExpression Condition { get; set; }
}

public class SubqueryEqualsExpression : ConditionExpression
{
    public IdExpression ColumnExpression { get; set; } = null!;
    public SelectExpression SelectExpression { get; set; } = null!;
}

public class SubqueryInExpression : ConditionExpression
{
    public IdExpression ColumnExpression { get; set; } = null!;
    public SelectExpression SelectExpression { get; set; } = null!;
}

public class LikeExpression : ConditionExpression
{
    public IdExpression ColumnName { get; set; } = null!;
    public ValueExpression Value { get; set; } = null!;
}

public class BetweenExpression : ConditionExpression
{
    public IdExpression ColumnName { get; set; } = null!;
    public ValueExpression LeftExpression { get; set; }
    public ValueExpression RightExpression { get; set; }
}

public class InRangeExpression : ConditionExpression
{
    public IdExpression ColumnName { get; set; }
    public List<ValueExpression> Values { get; set; } = new();
}

public class LessThanExpression : ConditionExpression
{
    public ValueExpression LeftExpression { get; set; }
    public ValueExpression RightExpression { get; set; }
}

public class GreaterThanExpression : ConditionExpression
{
    public ValueExpression LeftExpression { get; set; }
    public ValueExpression RightExpression { get; set; }
}

public class EqualExpression : ConditionExpression
{
    public ValueExpression LeftExpression { get; set; }
    public ValueExpression RightExpression { get; set; }
}

public class AndConditionExpression : ConditionExpression
{
    public ConditionExpression LeftExpression { get; set; }
    public ConditionExpression RightExpression { get; set; }
}

public class OrConditionExpression : ConditionExpression
{
    public ConditionExpression LeftExpression { get; set; }
    public ConditionExpression RightExpression { get; set; }
}