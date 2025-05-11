using System;
using System.Linq;
using System.Text;
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
        
        foreach (var column in columnsAndDef) 
        {
            if(column.ColumnDef!.ColumnDefType == ColumnDefType.Bool)
                await WriteBool(column.Value);
            else if(column.ColumnDef.ColumnDefType == ColumnDefType.Int)
                await WriteInt(column.Value);
            else if (column.ColumnDef.ColumnDefType == ColumnDefType.Text)
                await WriteText(column.Value);
        }
    }

    private Task WriteInt(object obj)
    {
        if(obj is not int i && (obj is double d && d % 1 != 0))
            throw new ArgumentException("Object is not an integer");

        var intValue = Convert.ToInt32(obj);
        
        return _tableStorage.FixedSizeStorage.Write(null, BitConverter.GetBytes(intValue));
    }
    
    private Task WriteBool(object obj)
    {
        if(obj is not bool b)
            throw new ArgumentException("Object is not an bool");
        
        return _tableStorage.FixedSizeStorage.Write(null, BitConverter.GetBytes(b));
    }
    
    private async Task WriteText(object obj)
    {
        if(obj is not string s)
            throw new ArgumentException("Object is not an string");
        
        var unicodeBytes = Encoding.Unicode.GetBytes(s);
        var metaDataAddress = await _tableStorage.VariableSizeStorage.Write(null, BitConverter.GetBytes(unicodeBytes.Length));
        await _tableStorage.VariableSizeStorage.Write(null, unicodeBytes);
        await _tableStorage.FixedSizeStorage.Write(null, BitConverter.GetBytes(metaDataAddress));
    }

    public async Task Flush()
    {
        await _tableStorage.FixedSizeStorage.Flush();
        await _tableStorage.VariableSizeStorage.Flush();
    }
}