using System;
using System.Collections.Generic;
using PoolSystem.PoolSystem;

namespace PoolSystem
{
    public static class PoolAccess
    {
        private static Dictionary<Type, IPoolBase> _pools = new();

        public static void SetUp(Dictionary<Type, IPoolBase> poolMap)
        {
            _pools = poolMap;
        }

        public static T Get<T>() where T : IPoolBase
        {
            if (_pools.TryGetValue(typeof(T), out var pool))
                return (T)pool;

            throw new KeyNotFoundException($"Pool of type {typeof(T).Name} not found.");
        }
    }
}