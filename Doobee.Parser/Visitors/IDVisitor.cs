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

        public IdExpression VisitColumn_constraint([NotNull] Parser.Column_constraintContext context)
        {
            return new IdExpression();
        }

        public IdExpression VisitColumn_def([NotNull] Parser.Column_defContext context)
        {
            return new IdExpression();
        }

        public IdExpression VisitColumn_defs([NotNull] Parser.Column_defsContext context)
        {
            return new IdExpression();
        }

        public IdExpression VisitCreate_tbl_stmt([NotNull] Parser.Create_tbl_stmtContext context)
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

        public IdExpression VisitType_name([NotNull] Parser.Type_nameContext context)
        {
            return new IdExpression();
        }
    }
}
