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
            if (context.exception != null)
                throw new SqlParseException("Invalid create table statement");

            context.children[0].Accept(this);
            context.children[1].Accept(this);

            var idContext = context.table_name();
            if (idContext == null)
                throw new SqlParseException("There is no table name specified in the create table statement");

            var idExpression = idContext.Accept(new IDVisitor());

            var cols = context.column_defs().Accept(new ColumnDefsVisitor());
                


            return new CreateTableExpression(idExpression, cols);
        }

        public override CreateTableExpression VisitErrorNode([NotNull] IErrorNode node)
        {
            throw new SqlParseException("unable to correctly parse create table statement");
        }       
    }
}
