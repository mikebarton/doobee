using System.Linq;
using Doobee.Engine.Engine.Processing.Insert;
using Doobee.Engine.Schema;

namespace Doobee.Engine.Engine.Validation;

internal class MultiRowValidator
{
    private readonly RowValidator _rowValidator;
    
    public MultiRowValidator(string tableName, string[] columnNames, SchemaDef schemaDef)
    {
        _rowValidator = new (tableName, columnNames, schemaDef);
    }

    public ValidationResult Validate(ColumnValue[][] rows)
    {
        foreach (var row in rows)   
        {
            var validationResult = _rowValidator.Validate(row);
            if (!validationResult.IsValid)
                return validationResult;
        }

        return new ValidationResult(true, null);
    }
}