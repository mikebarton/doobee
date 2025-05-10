using System.Threading.Tasks;
using Doobee.Engine.Engine.Processing.Insert;

namespace Doobee.Engine.Storage;

public interface IEntityRecordPersistence
{
    Task WriteRecord(ColumnValue[] columnValues);
    Task Flush();
}