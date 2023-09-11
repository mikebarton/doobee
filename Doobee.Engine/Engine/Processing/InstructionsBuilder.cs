using Doobee.Engine.Engine.Processing;
using Doobee.Engine.Engine.Processing.CreateTable;
using Doobee.Parser;
using Doobee.Parser.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Doobee.Engine.Engine.Processing.CreateTable.CreateTableStatement;
using static Doobee.Engine.Engine.Processing.CreateTable.CreateTableStatement.ColumnPart;

namespace Doobee.Engine.Engine.Processing
{
    internal class InstructionsBuilder
    {
        private readonly SqlParser _sqlParser;
        public InstructionsBuilder(SqlParser parser)
        {
            _sqlParser = parser;
        }

        public List<Statement> Build(string[] statements)
        {
            var getDataType = (TypeExpression type) =>
                type switch
                {
                    IntTypeExpression exp => ColumnType.Int,
                    TextTypeExpression exp => ColumnType.Text,
                    BoolTypeExpression exp => ColumnType.Boolean,
                    _ => throw new NotSupportedException()
                };

            return statements.Select(x => _sqlParser.ParseStatement(x)).Select(expression =>
            {
                if (expression == null)
                    throw new ArgumentNullException("expression");

                if (!expression.IsCreateTableStatement)
                    throw new ArgumentException("expression is not a create table statement", "expression");

                var createExpression = (CreateTableExpression)expression;
                var columns = createExpression.ColumnDefs.ColumnDefs.Select(x =>
                    new ColumnPart(
                        x.ColumnName.Id,
                        x.Constraints.Any(y => y.NotNull || y.PrimaryKey),
                        x.Constraints.Any(y => y.PrimaryKey),
                        getDataType(x.ColumnType))
                    ).ToList();

                var result = new CreateTableStatement(createExpression.TableName.Id, columns);
                return result;
            }).Cast<Statement>().ToList();

        }
    }
}
