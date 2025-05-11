using System.Linq;
using Doobee.Engine.Engine.Processing.Insert;
using Doobee.Engine.Schema;

namespace Doobee.Engine.Engine.Validation;

internal class RowValidator
{
    private readonly string _tableName;
    private readonly string[] _columnNames;
    private readonly SchemaDef _schemaDef;
    
    public RowValidator(string tableName, string[] columnNames, SchemaDef schemaDef)
    {
        _tableName = tableName;
        _columnNames = columnNames;
        _schemaDef = schemaDef;
    }

    public ValidationResult Validate(ColumnValue[] rowValues)
    {
        var tableDef = _schemaDef.GetTable(_tableName);
        if(tableDef == null)
            return new ValidationResult(false,"Table not found");

        var columnDefs = _columnNames.Select(x => new { Name = x, Def = tableDef.GetColumn(x) }).ToList();
        var firstNullColumnDef = columnDefs.FirstOrDefault(x => x.Def == null);
        if(firstNullColumnDef != null)
            return new ValidationResult(false,$"Column '{firstNullColumnDef.Name}' not found on table '{tableDef.Name}'");
            
        var validators = columnDefs.Select(x=> new ColumnValidator(x.Def!)).ToList();

        foreach (var row in rowValues)
        {
            if (rowValues.Length != validators.Count)
                return new ValidationResult(false, 
                    $"number of values in row does not match number of columns specified in statement");

            for (int i = 0; i < rowValues.Length; i++)
            {
                var validationResult = validators[i].IsValid(rowValues[i].Value);
                if(!validationResult.IsValid)
                    return validationResult;
            }
        }
            
        return new ValidationResult(true,null);
    }
}