namespace Doobee.Parser.Expressions;

public class SelectExpression : ParseExpression
{
    public IdExpression TableName { get; set; }
    public bool SelectAll { get; set; }
    public List<IdExpression>? ColumnNames { get; set;}
    public int? TopCount { get; set; }
    public WhereExpression? WhereExpression { get; set; }
}