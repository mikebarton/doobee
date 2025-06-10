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

            foreach (var contextChild in context.children)
            {
                contextChild.Accept(this);
            }

            _id = context.table_name().Accept(new IDVisitor());
            _colNames = context.column_list().Accept(new ColumnListVisitor());
            _values = context.values_clause().Accept(new ValuesClauseVisitor());
            
            if (_id == null || _colNames == null || _values == null)
                throw new SqlParseException("unable to parse insert statement");
            if (!_values.All(x=>x.Values.Count == _colNames.Count))
                throw new SqlParseException("mismatching columns to values when parsing insert statement");

            return new InsertExpression { TableName = _id, ColumnNames = _colNames, ValuesExpressions = _values };
        }

        public override InsertExpression VisitErrorNode([NotNull] IErrorNode node)
        {
            throw new SqlParseException("unable to parse insert statement");
        }
    }
}
