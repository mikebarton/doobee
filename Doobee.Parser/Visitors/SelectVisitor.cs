using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;

namespace Doobee.Parser.Visitors;

public class SelectVisitor : DoobeeSqlParserBaseVisitor<SelectExpression>
{
    public IdExpression _tableName { get; set; }
    public bool _selectAll { get; set; }
    public List<IdExpression>? _columnNames { get; set; }
    public int? _topCount { get; set; }
    public WhereExpression? _whereExpression { get; set; }
    
    public override SelectExpression VisitSelect_stmt(DoobeeSqlParser.Select_stmtContext context)
    {
        if (context.exception != null)
            throw new SqlParseException("unable to parse insert statement");

        _tableName = context.table_name().Accept(new IDVisitor());
        _whereExpression = context.where_clause()?.Accept(new WhereVisitor());

        foreach (var child in context.children)
        {
            child.Accept(this);
        }
        
        if(_tableName == null || 
           (_selectAll && _columnNames != null) || 
           (!_selectAll && _columnNames == null))
            throw new SqlParseException("unable to parse insert statement");
        
        return new SelectExpression()
        {
            TableName = _tableName!,
            TopCount = _topCount,
            SelectAll = _selectAll,
            ColumnNames = _columnNames,
            WhereExpression = _whereExpression,
        };
    }

    public override SelectExpression VisitSelect_columns(DoobeeSqlParser.Select_columnsContext context)
    {
        _columnNames = context.column_list()?.Accept(new ColumnListVisitor());
        _selectAll = context.STAR()?.GetText() == "*";
        return base.VisitSelect_columns(context);
    }

    public override SelectExpression VisitTop_count(DoobeeSqlParser.Top_countContext context)
    {
        if(context.exception != null || context.ChildCount != 2)
            throw new SqlParseException("unable to parse select statement");

        var intText = context.NUMERIC_LITERAL().GetText();
        if(!int.TryParse(intText, out var topCount))
            throw new SqlParseException("unable to parse select statement top count");
        
        _topCount = topCount;
        return base.VisitTop_count(context
        );
    }
    
    public override SelectExpression VisitErrorNode([NotNull] IErrorNode node)
    {
        throw new SqlParseException("unable to parse select statement");
    }
}