using CoreLayer.Utilities.Cache.Abstrack;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.Cache.Concreate
{
    public class RedisCacheWorkFlow : ICacheWorkFlow
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private readonly string _host;
        private readonly int _port;
        private readonly int _db;
        public RedisCacheWorkFlow(string host, int port, int db = 1)
        {
            _host = host;
            _port = port;
            _db = db;
            _connectionMultiplexer = ConnectionMultiplexer.Connect($"{host}:{port}");
            _database = _connectionMultiplexer.GetDatabase(db);
        }
        public void Add<T>(string key, T data, double cacheTime)
        {
            _database.StringSet(key, JsonConvert.SerializeObject(data));
        }

        public void Add<T>(string key, T data, DateTime cacheDateTime)
        {
            TimeSpan timeSpan = cacheDateTime - DateTime.Now;
            _database.StringSet(key, JsonConvert.SerializeObject(data), timeSpan);
        }

        public T Get<T>(string key)
        {
            if (_database.KeyExists(key))
                return JsonConvert.DeserializeObject<T>(_database.StringGet(key));
            else
                return default(T);
        }

        public object Get(string key)
        {
            if (_database.KeyExists(key))
                return JsonConvert.DeserializeObject<object>(_database.StringGet(key));
            else
                return null;
        }

        public bool IsAdd(string key)
        {
            return _database.KeyExists(key);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var server = _connectionMultiplexer.GetServer(_host,_port);
            foreach (var key in server.Keys(pattern: pattern))
            {
                _database.KeyDelete(key);
            }
        }
    }
}
