using System;

namespace CoreLayer.Utilities.Cache.Abstrack
{
    public interface ICacheWorkFlow
    {
        T Get<T>(string key);
        object Get(string key);
        void Add<T>(string key, T data, double cacheTime);
        void Add<T>(string key, T data, DateTime cacheDateTime);
        bool IsAdd(string key);
        void Remove(string key);
        void RemoveByPattern(string pattern);
    }
}
