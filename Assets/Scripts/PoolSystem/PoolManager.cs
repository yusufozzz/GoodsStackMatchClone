using System;
using System.Collections.Generic;
using GameManagement.Managers;
using PoolSystem.PoolSystem;
using UnityEngine;

namespace PoolSystem
{
    public class PoolManager : ManagerBase
    {
        private Dictionary<Type, IPoolBase> _poolMap;

        public override void SetUp()
        {
            base.SetUp();

            _poolMap = new Dictionary<Type, IPoolBase>();

            foreach (var pool in GetComponentsInChildren<IPoolBase>())
            {
                if (pool is not IPoolBase validPool) continue;

                var type = pool.GetType();
                if (!_poolMap.ContainsKey(type))
                {
                    _poolMap[type] = validPool;
                }
                else
                {
                    Debug.LogWarning($"Duplicate pool type: {type.Name}");
                }
            }

            PoolAccess.SetUp(_poolMap);

            foreach (var pool in _poolMap.Values)
            {
                pool.InitializePools();
            }
        }
    }
}