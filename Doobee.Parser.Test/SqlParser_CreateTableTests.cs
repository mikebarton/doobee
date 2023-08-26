using Doobee.Parser.Expressions;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Parser.Test
{
    [TestFixture]
    internal class SqlParser_CreateTableTests
    {
        [Test]
        public void CreateTable_RecordsTableName()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id int primary key)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.TableName.Id.ShouldBe("foo");
        }

        [Test]
        public void WhenHasNoTableName_ThrowsSqlParseException()
        {
            var parser = new SqlParser();
            Should.Throw<SqlParseException>(() => parser.ParseStatement("create table (id int primary key)"));            
        }
    }
}
