using Doobee.Parser.Expressions;
using NUnit.Framework;
using Shouldly;

namespace Doobee.Parser.Test;

[TestFixture]
public class SqlParser_SelectStatementTests
{
    [Test]
    public void WhenSelectStatementParsed_ExpressionIsSelectExpression()
    {
        var parser = new SqlParser();
        var expression = parser.ParseStatement("select * from student");
        expression.IsSelectExpression.ShouldBeTrue();
        var selectExpression = expression as SelectExpression;
        selectExpression.ShouldNotBeNull();
    }
    
    [Test]
    public void WhenSelectStar_ColumnsAreNull()
    {
        var parser = new SqlParser();
        var expression = parser.ParseStatement("select * from student");
        var selectExpression = expression as SelectExpression;
        selectExpression.ShouldNotBeNull();
        selectExpression.SelectAll.ShouldBeTrue();
        selectExpression.ColumnNames.ShouldBeNull();
    }

    [Test]
    public void WhenSelectOneColumns_SelectAllIsFalse()
    {
        var parser = new SqlParser();
        var expression = parser.ParseStatement("select one from student");
        var selectExpression = expression as SelectExpression;
        selectExpression.ShouldNotBeNull();
        selectExpression.SelectAll.ShouldBeFalse();
        selectExpression.ColumnNames.ShouldNotBeNull();
        selectExpression.ColumnNames.Count.ShouldBe(1);
        selectExpression.ColumnNames[0].Id.ShouldBe("one");
    }

    [Test]
    public void WhenSelectMultipleColumns_SelctAllIsFalse()
    {
        var parser = new SqlParser();
        var expression = parser.ParseStatement("select one,two,three from student");
        var selectExpression = expression as SelectExpression;
        selectExpression.ShouldNotBeNull();
        selectExpression.SelectAll.ShouldBeFalse();
        selectExpression.ColumnNames.ShouldNotBeNull();
        selectExpression.ColumnNames.Count.ShouldBe(3);
        selectExpression.ColumnNames[0].Id.ShouldBe("one");
        selectExpression.ColumnNames[1].Id.ShouldBe("two");
        selectExpression.ColumnNames[2].Id.ShouldBe("three");
    }

    [Test]
    public void WhenSelectFromTable_TableNameIsParsed()
    {
        var parser = new SqlParser();
        var expression = parser.ParseStatement("select one,two,three from student");
        var selectExpression = expression as SelectExpression;
        selectExpression.ShouldNotBeNull();
        selectExpression.TableName.Id.ShouldBe("student");
    }

    [Test]
    public void WhenSelectWithoutTopCount_TopCountIsNull()
    {
        var parser = new SqlParser();
        var expression = parser.ParseStatement("select one,two,three from student");
        var selectExpression = expression as SelectExpression;
        selectExpression.ShouldNotBeNull();
        selectExpression.TopCount.ShouldBeNull();
    }
    
    [Test]
    public void WhenSelectWithTopCount_TopCountIsSet()
    {
        var parser = new SqlParser();
        var expression = parser.ParseStatement("select top 11 one,two,three from student");
        var selectExpression = expression as SelectExpression;
        selectExpression.ShouldNotBeNull();
        selectExpression.TopCount.ShouldBe(11);
    }
    
    [TestCase("top abc")]
    [TestCase("top")]
    [TestCase("top 10df")]
    public void WhenSelectWithInvalidTopCount_ExceptionIsThrown(string topText)
    {
        var parser = new SqlParser();
        Should.Throw<SqlParseException>(() => parser.ParseStatement($"select {topText} one from student"));
    }
}