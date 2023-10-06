using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Visitors
{
    internal class InsertStatementVisitor : DoobeeSqlParserBaseVisitor<InsertExpression>
    {
        private IdExpression? _id;
        private List<IdExpression>? _colNames;
        private List<ValuesListExpression>? _values;

        public override InsertExpression VisitInsert_stmt([NotNull] DoobeeSqlParser.Insert_stmtContext context)
        {
            if (context.exception != null)
                throw new SqlParseException("unable to parse insert statement");

            foreach (var e in context.children)
            {
                e.Accept(this);
            }

            if (_id == null || _colNames == null || _values == null)
                throw new SqlParseException("unable to parse insert statement");
            if (!_values.All(x=>x.Values.Count == _colNames.Count))
                throw new SqlParseException("mismatching columns to values when parsing insert statement");

            return new InsertExpression { TableName = _id, ColumnNames = _colNames, ValuesExpressions = _values };
        }

        public override InsertExpression VisitValues_clause([NotNull] DoobeeSqlParser.Values_clauseContext context)
        {
            context.children[0].Accept(this);
            _values = context.value_row().Select(x=>x.Accept(new ValueExpressionVisitor())).ToList();
            return null;
        }

        public override InsertExpression VisitColumn_list([NotNull] DoobeeSqlParser.Column_listContext context)
        {
            var hasValidNumberChildren = context.children.Count == 1 || context.children.Count%2 == 1;
            if (!hasValidNumberChildren)
                throw new SqlParseException("Invalid number of arguments in column list");

            _colNames = context.column_name().Select(x => x.ID().Accept(new IDVisitor())).ToList();
            return null;
        }

        public override InsertExpression VisitTable_name([NotNull] DoobeeSqlParser.Table_nameContext context)
        {
            _id = context.Accept(new IDVisitor());
            return null;
        }

        public override InsertExpression VisitLiteral_value([NotNull] DoobeeSqlParser.Literal_valueContext context)
        {
            return base.VisitLiteral_value(context);
        }

        public override InsertExpression VisitTerminal([NotNull] ITerminalNode node)
        {
            return base.VisitTerminal(node);
        }

        

        public override InsertExpression VisitErrorNode([NotNull] IErrorNode node)
        {
            throw new SqlParseException("unable to parse insert statement");
        }
    }
}
