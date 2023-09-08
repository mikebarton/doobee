using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Expressions;
using Doobee.Parser.Listeners;
using Doobee.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser
{
    public class SqlParser
    {
        public ParseExpression ParseStatement(string sqlStatement)
        {
            AntlrInputStream inputStream = new AntlrInputStream(sqlStatement);
            var lexer = new DoobeeSqlLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new DoobeeSqlParser(tokens);
            parser.BuildParseTree = true;
            var tree = parser.parse();
           
            var expressionBuilder = new ExpressionBuilder();
            ParseTreeWalker.Default.Walk(expressionBuilder, tree);
            if (expressionBuilder.Expression == null)
                throw new SqlParseException("Invalid Statement. Cannot Parse");

            return expressionBuilder.Expression;
        }
    }
}
