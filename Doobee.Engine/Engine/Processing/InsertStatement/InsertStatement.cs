using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doobee.Parser.Expressions;

namespace Doobee.Engine.Engine.Processing.Insert
{
    internal class InsertStatement : Statement
    {
        public InsertStatement(InsertExpression expression)
        {
            TableName = expression.TableName.Id;
            ColumnNames = expression.ColumnNames.Select(x=>x.Id).ToArray();
            RowValues = expression.ValuesExpressions.Select(r =>
            {
                return r.Values.Zip(expression.ColumnNames)
                    .Select(p => new ColumnValue(p.Second.Id, p.First.Value))
                    .ToArray();
            }).ToArray();
        }
        
        public string TableName { get; }
        public string[] ColumnNames { get; }
        public ColumnValue[][] RowValues { get; }
    }
    
    public record ColumnValue(string ColumnName, object Value);
}
