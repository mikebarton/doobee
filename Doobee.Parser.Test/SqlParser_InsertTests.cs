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
    internal class SqlParser_InsertTests
    {
        [Test]
        public void WhenParseInsertState_ReadTableNameCorrectly()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("insert into Test (one, two, three) values (1, 2, 'three')");
            expression.IsInsertExpression.ShouldBeTrue();

            var insertExpression = expression as InsertExpression;
            insertExpression.ShouldNotBeNull();
            insertExpression.TableName.Id.ShouldBe("Test");
        }


        [Test]
        public void WhenParseInsertState_IncludeCorrectNumberColumns()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("insert into Test (one, two, three) values (1, 2, 'three')");
            expression.IsInsertExpression.ShouldBeTrue();

            var insertExpression = expression as InsertExpression;
            insertExpression.ShouldNotBeNull();
            insertExpression.ColumnNames.Count.ShouldBe(3);
            insertExpression.ValuesExpressions.Count.ShouldBe(1);
            insertExpression.ValuesExpressions[0].Values.Count.ShouldBe(3);
        }

        [Test]
        public void WhenParseInsertState_ParseColumnNamesCorrectly()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("insert into Test (one, two, three) values (1, 2, 'three'), (1, false, 'four')");
            expression.IsInsertExpression.ShouldBeTrue();

            var insertExpression = expression as InsertExpression;
            insertExpression.ShouldNotBeNull();
            insertExpression.ColumnNames.Count.ShouldBe(3);
            insertExpression.ColumnNames[0].Id.ShouldBe("one");
            insertExpression.ColumnNames[1].Id.ShouldBe("two");
            insertExpression.ColumnNames[2].Id.ShouldBe("three");
        }

        [Test]
        public void WhenParseInsertStatement_ParseMultipleValueRows()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("insert into Test (one, two, three) values (1, 2, 'three'), (1, false, 'four')");
            expression.IsInsertExpression.ShouldBeTrue();

            var insertExpression = expression as InsertExpression;
            insertExpression.ShouldNotBeNull();
            insertExpression.ValuesExpressions.Count().ShouldBe(2);
            Assert.AreEqual(insertExpression.ValuesExpressions[0].Values[0].Value, 1);
            Assert.AreEqual(insertExpression.ValuesExpressions[0].Values[1].Value, 2);
            Assert.AreEqual(insertExpression.ValuesExpressions[0].Values[2].Value, "three");
            Assert.AreEqual(insertExpression.ValuesExpressions[1].Values[0].Value, 1);
            Assert.AreEqual(insertExpression.ValuesExpressions[1].Values[1].Value, false);
            Assert.AreEqual(insertExpression.ValuesExpressions[1].Values[2].Value, "four");
        }

        [Test]
        public void WhenParseInsert_ShouldParseCorrectTextValue()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("insert into Test (three) values ('three')");
            expression.IsInsertExpression.ShouldBeTrue();

            var insertExpression = expression as InsertExpression;
            insertExpression.ShouldNotBeNull();
            Assert.IsTrue(insertExpression.ValuesExpressions[0].Values[0].Value is string);
            Assert.AreEqual(insertExpression.ValuesExpressions[0].Values[0].Value, "three");
        }

        [Test]
        public void WhenParseInsert_ShouldParseCorrectFalseBoolValue()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("insert into Test (three) values (false)");
            expression.IsInsertExpression.ShouldBeTrue();

            var insertExpression = expression as InsertExpression;
            insertExpression.ShouldNotBeNull();
            Assert.IsTrue(insertExpression.ValuesExpressions[0].Values[0].Value is bool);
            Assert.IsFalse(insertExpression.ValuesExpressions[0].Values[0].Value);
        }

        [Test]
        public void WhenParseInsert_ShouldParseCorrectTrueBoolValue()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("insert into Test (three) values (true)");
            expression.IsInsertExpression.ShouldBeTrue();

            var insertExpression = expression as InsertExpression;
            insertExpression.ShouldNotBeNull();
            Assert.IsTrue(insertExpression.ValuesExpressions[0].Values[0].Value is bool);
            Assert.IsTrue(insertExpression.ValuesExpressions[0].Values[0].Value);
        }

        [Test]
        public void WhenParseInsert_ShouldParseCorrectNumberValue()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("insert into Test (three) values (4)");
            expression.IsInsertExpression.ShouldBeTrue();

            var insertExpression = expression as InsertExpression;
            insertExpression.ShouldNotBeNull();
            Assert.IsTrue(insertExpression.ValuesExpressions[0].Values[0].Value is double);
            Assert.AreEqual(insertExpression.ValuesExpressions[0].Values[0].Value, 4);
        }

        [Test]
        [TestCase("insert into Test (alpha one, two, three) values (1, false, 'true')")]
        [TestCase("insert into Test (one, alpha two, three) values (1, false, 'true')")]
        [TestCase("insert into ONE Test (one, two, three) values (1, false, 'true')")]
        [TestCase("insert into Test (one, two, three) other values (1, false, 'true')")]
        [TestCase("insert into Test (one, two, three) values other (1, false, 'true')")]
        [TestCase("insert into Test (one, two, three) values (true 1, false, 'true')")]
        [TestCase("insert into Test (one, two, three) values (1, false, 'true' 'other')")]
        [TestCase("insert into Test (one, two, three) values (1, false, other)")]
        [TestCase("insert into Test (one, two, three) values (1, false, 'true'")]
        [TestCase("insert into Test (one, two, three) values 1, false, 'true')")]
        [TestCase("insert into Test (one, two, three values (1, false, 'true')")]
        [TestCase("insert into Test one, two, three) values (1, false, 'true')")]
        public void WhenHasInvalidSql_ThrowsSqlParseException(string sql)
        {
            var parser = new SqlParser();
            Should.Throw<SqlParseException>(() => parser.ParseStatement(sql));
        }
    }
}
