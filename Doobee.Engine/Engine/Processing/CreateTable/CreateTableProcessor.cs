using Doobee.Engine.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine.Processing.CreateTable
{
    internal class CreateTableProcessor : DdlProcessor<CreateTableStatement>
    {
        protected override async Task<Response> ProcessConcrete(CreateTableStatement value, SchemaManager schemaManager)
        {
            if (schemaManager.Schema.GetTable(value.TableName) != null)
                return new CreateTableResponse($"Table {value.TableName} already exists", false);

            var newTable = schemaManager.Schema.AddTable(value.TableName);
            foreach (var col in value.Columns)
            {
                var newCol = newTable.AddColumn(col.ColumnName);
                newCol.Nullable = !col.NotNull;
                newCol.PrimaryKey = col.IsPrimaryKey;
            }

            await schemaManager.Save();
            return new CreateTableResponse($"Table {value.TableName} successfully created", true);
        }
    }
}
