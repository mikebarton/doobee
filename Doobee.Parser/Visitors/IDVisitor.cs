using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Visitors
{
    internal class IDVisitor : IDoobeeSqlParserVisitor<IdExpression>
    {
        public IdExpression Visit([NotNull] IParseTree tree)
        {
            return new IdExpression();
        }

        public IdExpression VisitChildren([NotNull] IRuleNode node)
        {
            return new IdExpression();
        }

        public IdExpression VisitColumn_constraint([NotNull] DoobeeSqlParser.Column_constraintContext context)
        {
            return new IdExpression();
        }

        public IdExpression VisitColumn_def([NotNull] DoobeeSqlParser.Column_defContext context)
        {
            return new IdExpression();
        }

        public IdExpression VisitColumn_defs([NotNull] DoobeeSqlParser.Column_defsContext context)
        {
            return new IdExpression();
        }

        public IdExpression VisitCreate_tbl_stmt([NotNull] DoobeeSqlParser.Create_tbl_stmtContext context)
        {
            return new IdExpression();
        }

        public IdExpression VisitErrorNode([NotNull] IErrorNode node)
        {
            return new IdExpression();
        }

        public IdExpression VisitTerminal([NotNull] ITerminalNode node)
        {
            return new IdExpression() { Id = node.GetText() };
        }

        public IdExpression VisitType_name([NotNull] DoobeeSqlParser.Type_nameContext context)
        {
            return new IdExpression();
        }
    }
}
