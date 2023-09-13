using Doobee.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Storage
{
    internal class JsonDataRepo
    {
        private readonly IDataStorage _storage;
        public JsonDataRepo(IDataStorage storage)
        {
            _storage = storage;
        }

        public async Task<T?> Read<T>()
        {
            var bytes = await _storage.Read(0, _storage.EndOfFileAddress);

            if (bytes == null || !bytes.Any())
                return default(T);
            
            var text = Encoding.UTF8.GetString(bytes);

            if (text == null)
                throw new Exception($"cannot deserialize an empty string when reading object of type {typeof(T).FullName}");

            var result = JsonConvert.DeserializeObject<T>(text!);
            if (result == null)
                throw new Exception($"deserializing file to object of type {typeof(T).FullName} returns null");

            return result;
        }

        public async Task Write<T>(T value)
        {
            var text = JsonConvert.SerializeObject(value);
            var bytes = Encoding.UTF8.GetBytes(text);
            await _storage.Clear();
            await _storage.Write(0, bytes);
        }
    }
}
