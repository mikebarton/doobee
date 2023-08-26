using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Doobee.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            string discount = "create table foo(id INT primary key, words TEXT null, isgood bool not null)";
            AntlrInputStream inputStream = new AntlrInputStream(discount);
            var lexer = new DoobeeSqlLexer(inputStream);


            var tokens = new CommonTokenStream(lexer);
            var parser = new DoobeeSqlParser(tokens);
            parser.BuildParseTree = true;
            var tree = parser.create_tbl_stmt();
            var expression = tree.Accept(new CreateTableVisitor());

        }
    }
}
