using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Doobee.Engine.Engine;
using Doobee.Persistence;

namespace Doobee.Engine.Storage;

internal class EntityPersistence
{
    private readonly IDataStorageProvider _storageProvider;
    private DatabaseEntities? _entities;
    private Lazy<JsonDataRepo> _entitiesStorage;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    public EntityPersistence(IDataStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;            
    }
    
    public async Task Initialise(DatabaseConfiguration config)
    {
        _entitiesStorage = new Lazy<JsonDataRepo>(()=> new JsonDataRepo(_storageProvider.GetItemStorage(config.EngineId)));
        _entities = await _entitiesStorage.Value.Read<DatabaseEntities>();
        if(_entities == null)
        {
            _entities = new DatabaseEntities() { Id = config.EngineId };
            await _entitiesStorage.Value.Write<DatabaseEntities>(_entities);
        }
    }

    public async ValueTask<IDataStorage> GetOrAddStorage(DatabaseEntities.DatabaseEntity.DatabasesEntityType entityType, Guid? id = null)
    {
        var entityMetaData = _entities.Entities.SingleOrDefault(x => x.Type == entityType && (id == null || x.Id == id));
        if (entityMetaData == null)
        {
            await _semaphore.WaitAsync();
            try
            {
                entityMetaData = _entities.Entities.SingleOrDefault(x => x.Type == entityType && (id == null || x.Id == id));
                if (entityMetaData == null)
                {
                    entityMetaData = new DatabaseEntities.DatabaseEntity()
                    {
                        Id = id.HasValue ? id.Value : Guid.NewGuid(),
                        Type = entityType
                    };
                    _entities.Entities.Add(entityMetaData);
                    await _entitiesStorage.Value.Write(_entities);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        return _storageProvider.GetItemStorage(entityMetaData.Id);
    }
}