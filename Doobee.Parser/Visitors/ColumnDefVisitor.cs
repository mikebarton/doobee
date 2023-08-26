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
    internal class ColumnDefVisitor : DoobeeSqlParserBaseVisitor<ColumnDefExpression>
    {      

        public override ColumnDefExpression VisitColumn_def([NotNull] DoobeeSqlParser.Column_defContext context)
        {
            var colName = context.ID().Accept(new IDVisitor());
            var typeExpression = context.type_name().Accept(new TypeVisitor());
            var constraints = context.column_constraint().Select(x=> x.Accept(new ColumnConstraintVisitor())).ToList();
            return new ColumnDefExpression(colName, typeExpression, constraints);
        }

        
    }
}