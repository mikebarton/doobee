using Antlr4.Runtime;
using Doobee.Parser.Expressions;
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
            var tree = parser.create_tbl_stmt();
            if (tree.exception != null)
                throw new SqlParseException("the supplied sql statement was invalid");
            try
            {
                var expression = tree.Accept(new CreateTableVisitor());               
                return expression;
            }
            catch(Exception e)
            {
                throw new SqlParseException("an unknown error occurred while parsing sql");
            }
        }
    }
}
