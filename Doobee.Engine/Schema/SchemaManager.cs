using Doobee.Engine.Storage;
using Doobee.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Schema
{
    internal class SchemaManager
    {
        private readonly JsonDataRepo _storage;
        private SchemaDef _schema;

        public SchemaManager(IDataStorage storage)
        {
            _storage = new JsonDataRepo(storage);
        }

        public SchemaDef Schema => _schema;

        public async Task Load()
        {
            if (_schema == null)
            {
                _schema = await _storage.Read<SchemaDef>();

                if(_schema == null)
                {
                    _schema = new SchemaDef();
                    await _storage.Write(_schema);
                }
            }
        }

        public async Task Save()
        {
            if (_schema == null)
                throw new Exception("Cannot save schema when it has not already been loaded");

            await _storage.Write(_schema); 
        }
    }
}
