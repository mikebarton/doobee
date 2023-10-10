using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine.Processing.CreateTable
{
    internal class CreateTableStatement : Statement
    {
        public CreateTableStatement(string tableName, IReadOnlyList<ColumnPart> columns)
        {
            TableName = tableName;
            Columns = columns;
        }

        public string TableName { get; }
        public IReadOnlyList<ColumnPart> Columns { get; }

        public class ColumnPart
        {
            public ColumnPart(string columnName, bool notNull, bool isPrimaryKey, ColumnType dataType, int storageSizeBytes)
            {
                ColumnName = columnName;
                NotNull = notNull;
                IsPrimaryKey = isPrimaryKey;
                DataType = dataType;
                StorageSizeBytes = storageSizeBytes;
            }

            public string ColumnName { get; init; }
            public bool NotNull { get; init; }
            public bool IsPrimaryKey { get; init; }
            public ColumnType DataType { get; init; }

            public enum ColumnType
            {
                Int,
                Text,
                Boolean
            }

            public int StorageSizeBytes { get; init; }
        }
    }
}
