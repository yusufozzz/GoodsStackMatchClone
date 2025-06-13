using System;
using System.Collections.Generic;
using GameManagement.Managers;

namespace GameManagement
{
    public static class ManagerAccess
    {
        private static Dictionary<Type, ManagerBase> _managers = new();

        public static void SetUp(Dictionary<Type, ManagerBase> managerMap)
        {
            _managers = managerMap;
        }

        // Retrieves a manager of the specified type.
        public static T Get<T>() where T : ManagerBase
        {
            if (_managers.TryGetValue(typeof(T), out var manager))
                return (T)manager;

            throw new KeyNotFoundException($"Manager of type {typeof(T).Name} not found.");
        }
    }
}