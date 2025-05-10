using System;
using System.Linq;
using System.Threading.Tasks;
using Doobee.Engine.Engine.Processing.Insert;
using Doobee.Engine.Schema;
using Doobee.Persistence;

namespace Doobee.Engine.Storage;

internal class EntityRecordPersistence : IEntityRecordPersistence
{
    private readonly TableDef _table;
    private readonly TableStorage _tableStorage;
    
    public EntityRecordPersistence(TableDef tableDef, TableStorage tableStorage)
    {
        _table = tableDef;
        _tableStorage = tableStorage;
    }


    public async Task WriteRecord(ColumnValue[] columnValues)
    {
        var columnsAndDef = columnValues.Select(x => new
        {
            ColumnDef = _table.GetColumn(x.ColumnName),
            Value = x.Value,
        }).ToList();

        if (columnsAndDef.Any(x => x.ColumnDef is null))
            throw new InvalidOperationException("There is not ColumnDef defined for this ColumnValue");
        
        var writeAddress = _tableStorage.FixedSizeStorage.EndOfFileAddress;
        var totalFixedRowSize = columnsAndDef.Sum(x => x.ColumnDef!.StorageSize);
        await _tableStorage.FixedSizeStorage.Write(null, BitConverter.GetBytes(totalFixedRowSize));
        // writeAddress += sizeof(int); //totalFixedRowSize
        foreach (var column in columnsAndDef) 
        {
            _tableStorage.FixedSizeStorage.Write(null, BitConverter.GetBytes(column.ColumnDef!.StorageSize));
        }
    }

    private dynamic ConvertValue(object value, ColumnDefType columnDefType)
    {
        return columnDefType switch 

        {
            ColumnDefType.Text => 
        }
    }

    public Task Flush()
    {
        throw new System.NotImplementedException();
    }
}