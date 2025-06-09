using Antlr4.Runtime.Misc;
using Doobee.Parser.Expressions;

namespace Doobee.Parser.Visitors;

public class ColumnListVisitor : DoobeeSqlParserBaseVisitor<List<IdExpression>>
{
    public override List<IdExpression> VisitColumn_list([NotNull] DoobeeSqlParser.Column_listContext context)
    {
        var hasValidNumberChildren = context.children.Count == 1 || context.children.Count%2 == 1;
        if (!hasValidNumberChildren)
            throw new SqlParseException("Invalid number of arguments in column list");

        var cols = context.column_name().Select(x => x.ID().Accept(new IDVisitor())).ToList();
        return cols;
    }
}