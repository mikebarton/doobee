using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doobee.Engine.Engine.Processing.CreateTable;
using Doobee.Parser.Expressions;

namespace Doobee.Engine.Engine.Processing
{
    internal abstract class Statement
    {
        public bool IsDdl() => this is CreateTableStatement;
        
        protected CreateTableStatement.ColumnPart.ColumnType GetDataType (TypeExpression type) =>
            type switch
            {
                IntTypeExpression exp => CreateTableStatement.ColumnPart.ColumnType.Int,
                TextTypeExpression exp => CreateTableStatement.ColumnPart.ColumnType.Text,
                BoolTypeExpression exp => CreateTableStatement.ColumnPart.ColumnType.Boolean,
                _ => throw new NotSupportedException()
            };
        
        protected int GetStorageSize(TypeExpression type) =>
            type switch
            {
                IntTypeExpression exp => sizeof(int),
                TextTypeExpression exp => sizeof(long),//text stores address in separate string file
                BoolTypeExpression exp => sizeof(bool),
                _ => throw new NotSupportedException()
            };
    }
}
