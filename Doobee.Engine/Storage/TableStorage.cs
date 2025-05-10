using Doobee.Persistence;

namespace Doobee.Engine.Storage;

internal class TableStorage
{
    public IDataStorage FixedSizeStorage { get; init; }
    public IDataStorage VariableSizeStorage { get; init; }
}