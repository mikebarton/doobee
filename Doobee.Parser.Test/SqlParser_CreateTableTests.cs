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
        public void CreateTable_ShouldCaptureColumnName()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id int primary key)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.ColumnDefs.ColumnDefs.Count.ShouldBe(1);
            var col = createTableExpression.ColumnDefs.ColumnDefs[0];
            col.ShouldNotBeNull();
            col.ColumnName.ShouldNotBeNull();
            col.ColumnName.Id.ShouldBe("id");            
        }

        [Test]
        public void CreateTable_ShouldCaptureColumnType()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id int primary key)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.ColumnDefs.ColumnDefs.Count.ShouldBe(1);
            var col = createTableExpression.ColumnDefs.ColumnDefs[0];
            col.ShouldNotBeNull();
            col.ColumnName.ShouldNotBeNull();
            col.ColumnType.ShouldBeAssignableTo<IntTypeExpression>();            
        }

        [Test]
        public void CreateTable_ShouldCapturePrimaryKeyTrue()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id int primary key)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.ColumnDefs.ColumnDefs.Count.ShouldBe(1);
            var col = createTableExpression.ColumnDefs.ColumnDefs[0];
            col.ShouldNotBeNull();
            col.ColumnName.ShouldNotBeNull();            
            col.Constraints.Count.ShouldBe(1);
            col.Constraints[0].PrimaryKey.ShouldBeTrue();
        }

        [Test]
        public void CreateTable_WhenNoNullSpecificationRecordFalse()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id int primary key)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.ColumnDefs.ColumnDefs.Count.ShouldBe(1);
            var col = createTableExpression.ColumnDefs.ColumnDefs[0];
            col.ShouldNotBeNull();
            col.ColumnName.ShouldNotBeNull();
            col.Constraints.Count.ShouldBe(1);
            col.Constraints[0].PrimaryKey.ShouldBeTrue();
            col.Constraints[0].NotNull.ShouldBeFalse();
        }

        [Test]
        public void CreateTable_WhenNotNullRecordNotNull()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id int not null)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.ColumnDefs.ColumnDefs.Count.ShouldBe(1);
            var col = createTableExpression.ColumnDefs.ColumnDefs[0];
            col.ShouldNotBeNull();
            col.ColumnName.ShouldNotBeNull();
            col.Constraints.Count.ShouldBe(1);
            col.Constraints[0].PrimaryKey.ShouldBeFalse();
            col.Constraints[0].NotNull.ShouldBeTrue();
        }

        [Test]
        public void CreateTable_WhenNullRecordNotNullFalse()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id int null)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.ColumnDefs.ColumnDefs.Count.ShouldBe(1);
            var col = createTableExpression.ColumnDefs.ColumnDefs[0];
            col.ShouldNotBeNull();
            col.ColumnName.ShouldNotBeNull();
            col.Constraints.Count.ShouldBe(1);
            col.Constraints[0].PrimaryKey.ShouldBeFalse();
            col.Constraints[0].NotNull.ShouldBeFalse();
        }

        [Test]
        public void CreateTable_WhenPKAndNotNullRecordBoth()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id int primary key not null)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.ColumnDefs.ColumnDefs.Count.ShouldBe(1);
            var col = createTableExpression.ColumnDefs.ColumnDefs[0];
            col.ShouldNotBeNull();
            col.ColumnName.ShouldNotBeNull();
            col.Constraints.Count.ShouldBe(2);
            col.Constraints.Any(x=>x.PrimaryKey == true).ShouldBeTrue();
            col.Constraints.Any(x=>x.NotNull == true).ShouldBeTrue();
        }

        [Test]
        public void CreateTable_WhenNoConstraintsRecordNone()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id int)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.ColumnDefs.ColumnDefs.Count.ShouldBe(1);
            var col = createTableExpression.ColumnDefs.ColumnDefs[0];
            col.Constraints.Count.ShouldBe(0);            
        }

        [Test]
        public void CreateTable_CanParseMultipleColumns()
        {
            var parser = new SqlParser();
            var expression = parser.ParseStatement("create table foo(id1 int, id2 text)");
            expression.IsCreateTableStatement.ShouldBeTrue();

            var createTableExpression = expression as CreateTableExpression;
            createTableExpression.ShouldNotBeNull();
            createTableExpression.ColumnDefs.ColumnDefs.Count.ShouldBe(2);
            createTableExpression.ColumnDefs.ColumnDefs.Any(x=>x.ColumnName.Id == "id1" && x.ColumnType is IntTypeExpression).ShouldBeTrue();
            createTableExpression.ColumnDefs.ColumnDefs.Any(x => x.ColumnName.Id == "id2" && x.ColumnType is TextTypeExpression).ShouldBeTrue();            
        }

        [Test]
        [TestCase("create table (id int primary key)")]
        [TestCase("create tablee (id int primary key)")]
        [TestCase("ceate table (id int primary key)")]
        [TestCase("create table (ids int primary key)")]
        [TestCase("create table foo id int)")]
        [TestCase("create table foo (id int")]
        [TestCase("create table foo id int")]
        public void WhenHasInvalidSql_ThrowsSqlParseException(string sql)
        {
            var parser = new SqlParser();
            Should.Throw<SqlParseException>(() => parser.ParseStatement(sql));            
        }

    }
}
