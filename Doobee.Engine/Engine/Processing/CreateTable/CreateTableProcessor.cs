using Doobee.Engine.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doobee.Engine.Storage;

namespace Doobee.Engine.Engine.Processing.CreateTable
{
    internal class CreateTableProcessor : DdlProcessor<CreateTableStatement>
    {
        private readonly SchemaManager _schemaManager;

        public CreateTableProcessor(SchemaManager schemaManager)
        {
            _schemaManager = schemaManager;
        }
        protected override async Task<Response> ProcessConcrete(CreateTableStatement value)
        {
            if (_schemaManager.Schema.GetTable(value.TableName) != null)
                return new CreateTableResponse($"Table {value.TableName} already exists", false);

            var getDataType = (CreateTableStatement.ColumnPart.ColumnType colType) =>
                colType switch
                {
                    CreateTableStatement.ColumnPart.ColumnType.Text => ColumnDefType.Text,
                    CreateTableStatement.ColumnPart.ColumnType.Int => ColumnDefType.Int,
                    CreateTableStatement.ColumnPart.ColumnType.Boolean => ColumnDefType.Bool,
                    _ => throw new Exception("cannot map create table column type to schema column type")
                };
            

            var newTable = _schemaManager.Schema.AddTable(value.TableName);
            foreach (var col in value.Columns)
            {
                var newCol = newTable.AddColumn(col.ColumnName);
                newCol.Nullable = !col.NotNull;
                newCol.PrimaryKey = col.IsPrimaryKey;
                newCol.ColumnDefType = getDataType(col.DataType);
                newCol.StorageSize = col.StorageSizeBytes;
                if(newCol.PrimaryKey)
                    newCol.Index = new IndexDef { Id = Guid.NewGuid() };
            }

            await _schemaManager.Save();
            return new CreateTableResponse($"Table {value.TableName} successfully created", true);
        }
    }
}
