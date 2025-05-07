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
using Doobee.Engine.Engine.Processing.Insert;
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
            return statements.Select(x => _sqlParser.ParseStatement(x)).Select<ParseExpression, Statement>(expression =>
            {
                if (expression == null)
                    throw new ArgumentNullException("expression");

                if (expression.IsCreateTableStatement)
                    return new CreateTableStatement((CreateTableExpression)expression);
                
                if(expression.IsInsertExpression)
                    return new InsertStatement((InsertExpression)expression);

                throw new InvalidCastException("Expression type is not supported");
            }).ToList();

        }
    }
}
