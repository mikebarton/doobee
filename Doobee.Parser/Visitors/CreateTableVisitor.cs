using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Doobee.Parser.Parser;

namespace Doobee.Parser.Visitors
{
    internal class CreateTableVisitor : DoobeeSqlParserBaseVisitor<CreateTableExpression>
    {       

        public override CreateTableExpression VisitCreate_tbl_stmt([NotNull] Parser.Create_tbl_stmtContext context)
        {
            var idExpression = context.ID().Accept(new IDVisitor());
            var cols = context.column_defs().column_def().Select(x=> x.Accept(new ColumnDefVisitor())).ToList();
            return new CreateTableExpression(idExpression, cols);
        }

        
    }
}
