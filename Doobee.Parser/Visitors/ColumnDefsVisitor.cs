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
    internal class ColumnDefsVisitor : DoobeeSqlParserBaseVisitor<ColumnDefsExpression>
    {
        public override ColumnDefsExpression VisitErrorNode([NotNull] IErrorNode node)
        {
            throw new SqlParseException("unable to parse column definitions");            
        }

        public override ColumnDefsExpression VisitColumn_defs([NotNull] DoobeeSqlParser.Column_defsContext context)
        {
            if (context.exception != null)
                throw new SqlParseException("Unable to parse the column definitions");

            context.children[0].Accept(this);
            
            var cols = context.column_def().Select(x => x.Accept(new ColumnDefVisitor())).ToList();

            context.children[2].Accept(this);

            return new ColumnDefsExpression(cols);
        }
    }
}
