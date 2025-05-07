using Doobee.Engine.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doobee.Engine.Engine.Validation;
using Doobee.Engine.Storage;

namespace Doobee.Engine.Engine.Processing.Insert
{
    internal class InsertProcessor : DmlProcessor<InsertStatement>
    {
        private readonly EntityPersistence _entityPersistence;

        public InsertProcessor(EntityPersistence entityPersistence)
        {
            _entityPersistence = entityPersistence;
        }
        
        protected override async Task<Response> ProcessConcrete(InsertStatement value, SchemaManager schemaManager)
        {
            var validator = new MultiRowValidator(value.TableName, value.ColumnNames, schemaManager.Schema);
            var validationResult = validator.Validate(value.RowValues);
            if(!validationResult.IsValid)
                return new InsertResponse(validationResult.ErrorMessage ?? "Invalid insert statement", false);
            
            var tableDef = schemaManager.Schema.GetTable(value.TableName);
            if(tableDef == null)
                return new InsertResponse("Table not found", false);
            
            var tableStorage = await _entityPersistence.GetOrAddStorage(DatabaseEntities.DatabaseEntity.DatabasesEntityType.Table, tableDef.Id);
            var tableStringStorage = await _entityPersistence.GetOrAddStorage(DatabaseEntities.DatabaseEntity.DatabasesEntityType.TableText, tableDef.Id);

            throw new NotImplementedException();
        }

        
    }
}
