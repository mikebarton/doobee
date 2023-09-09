﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Messages.Statements
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
            public ColumnPart(string columnName, bool notNull, bool isPrimaryKey, ColumnType dataType)
            {
                ColumnName = columnName;
                NotNull = notNull;
                IsPrimaryKey = isPrimaryKey;
                DataType = dataType;
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
        }
    }
}
