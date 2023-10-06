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
    internal class IDVisitor : DoobeeSqlParserBaseVisitor<IdExpression>
    {   
        public override IdExpression VisitTerminal([NotNull] ITerminalNode node)
        {
            return new IdExpression() { Id = node.GetText() };
        }        
    }
}
