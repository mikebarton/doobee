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
    internal class CreateTableVisitor : DoobeeSqlParserBaseVisitor<CreateTableExpression>
    {       

        public override CreateTableExpression VisitCreate_tbl_stmt([NotNull] DoobeeSqlParser.Create_tbl_stmtContext context)
        {
            var createBody = context.create_tbl_body();
            if (createBody.exception != null)
                throw new SqlParseException("invalid syntax for create table statement");
            
            var idContext = createBody.ID();
            if (idContext == null)
                throw new SqlParseException("There is no table name specified in the create table statement");

            var idExpression = idContext.Accept(new IDVisitor());
            var cols = createBody.column_defs().column_def().Select(x => x.Accept(new ColumnDefVisitor())).ToList();
            return new CreateTableExpression(idExpression, cols);
        }

        
    }
}
