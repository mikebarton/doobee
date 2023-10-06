using Antlr4.Runtime.Misc;
using Doobee.Parser.Expressions;
using Doobee.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Listeners
{
    internal class ExpressionBuilder : DoobeeSqlParserBaseListener
    {
        public override void EnterCreate_tbl_stmt([NotNull] DoobeeSqlParser.Create_tbl_stmtContext context)
        {
            Expression = context.Accept(new CreateTableVisitor());
        }

        public override void EnterInsert_stmt([NotNull] DoobeeSqlParser.Insert_stmtContext context)
        {
            Expression = context.Accept(new InsertStatementVisitor());
        }

        public ParseExpression? Expression { get; private set; } = null!;
    }
}
