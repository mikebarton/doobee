﻿using Doobee.Engine.Schema;
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
        private readonly SchemaManager _schemaManager;

        public InsertProcessor(EntityPersistence entityPersistence, SchemaManager schemaManager)
        {
            _entityPersistence = entityPersistence;
            _schemaManager = schemaManager;
        }
        
        protected override async Task<Response> ProcessConcrete(InsertStatement value)
        {
            var validator = new MultiRowValidator(value.TableName, value.ColumnNames, _schemaManager.Schema);
            var validationResult = validator.Validate(value.RowValues);
            if(!validationResult.IsValid)
                return new InsertResponse(validationResult.ErrorMessage ?? "Invalid insert statement", false);
            
            var tableDef = _schemaManager.Schema.GetTable(value.TableName);
            if(tableDef == null)
                return new InsertResponse("Table not found", false);
            
            var tableStorage = await _entityPersistence.GetTablePersistence(tableDef);
            foreach (var row in value.RowValues)
            {
                await tableStorage.WriteRecord(row);
            }

            await tableStorage.Flush();
            
            return new InsertResponse($"{value.RowValues.Length} successfully inserted", true);
        }

        
    }
}
