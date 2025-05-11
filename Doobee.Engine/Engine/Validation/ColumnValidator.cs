using System;
using System.ComponentModel;
using Doobee.Engine.Schema;

namespace Doobee.Engine.Engine.Validation;

internal class ColumnValidator
{
    private readonly ColumnDef _columnDef;
    
    public ColumnValidator(ColumnDef columnDef)
    {
        _columnDef = columnDef;
    }

    public ValidationResult IsValid(object? obj)
    {
        var result = ValidateNullability(obj);
        result ??= ValidateIntType(obj);
        result ??= ValidateBoolType(obj);
        result ??= ValidateTextType(obj);
        
        if(result != null)
            return result;
        
        return new ValidationResult(true, null);
    }

    private ValidationResult? ValidateBoolType(object? obj)
    {
        if (_columnDef.ColumnDefType != ColumnDefType.Bool)
            return null;
        
        if (obj == null)
            return null;

        if (obj is bool boolValue)
            return null;
        
        return new ValidationResult(false, $"Column '{_columnDef.Name}' can not be used as an bool.");
    }

    private ValidationResult? ValidateTextType(object? obj)
    {
        if (_columnDef.ColumnDefType != ColumnDefType.Text)
            return null;
        
        if (obj == null)
            return null;

        if (obj is string stringValue)
            return null;
        
        return new ValidationResult(false, $"Column '{_columnDef.Name}' can not be used as an string.");
    }

    private ValidationResult? ValidateIntType(object? obj)
    {
        if (_columnDef.ColumnDefType != ColumnDefType.Int)
            return null;
        
        if (obj == null)
            return null;
        
        if (obj is double dubs)
        {
            if (dubs % 1 == 0)
                return null;
        }
        
        if (obj is int intValue)
            return null;
        
        return new ValidationResult(false, $"Column '{_columnDef.Name}' can not be used as an integer.");
    }

    private ValidationResult? ValidateNullability(object? obj)
    {
        if (obj == null && !_columnDef.Nullable)
            return new ValidationResult(false, $"Column '{_columnDef.Name}' cannot be null.'");

        return null;
    }
}