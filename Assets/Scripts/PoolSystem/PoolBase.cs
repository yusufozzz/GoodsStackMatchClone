using System;
using System.Collections.Generic;
using PoolSystem.PoolSystem;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace PoolSystem
{
    public abstract class PoolBase<T> : MonoBehaviour, IPoolBase where T : Object
    {
        [SerializeField]
        protected T[] prefabs;

        [SerializeField]
        protected bool collectionChecks = true;

        [SerializeField]
        protected int defaultCapacity = 10;

        [SerializeField]
        protected int maxPoolSize = 100;

        private readonly Dictionary<int, IObjectPool<T>> _pools = new();
        private readonly Dictionary<T, int> _poolIndices = new();

        public virtual void InitializePools()
        {
            _pools.Clear();
            _poolIndices.Clear();

            for (int i = 0; i < prefabs.Length; i++)
            {
                int tempIndex = i;
                var pool = new ObjectPool<T>(
                    createFunc: () => OnCreate(tempIndex),
                    actionOnGet: OnGet,
                    actionOnRelease: OnRelease,
                    actionOnDestroy: HandleObjectDestruction,
                    collectionCheck: collectionChecks,
                    defaultCapacity: defaultCapacity,
                    maxSize: maxPoolSize
                );
                _pools.Add(tempIndex, pool);
            }
        }

        protected virtual T OnCreate(int index)
        {
            if (index < 0 || index >= prefabs.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index),
                    $"Attempted to access prefab at index {index}, but prefabs length is {prefabs.Length}.");
            }

            var instance = Instantiate(prefabs[index], Vector3.back * 50, Quaternion.identity, transform);
            _poolIndices[instance] = index;
            OnObjectCreated(instance, index);
            return instance;
        }

        protected virtual void OnObjectCreated(T instance, int poolIndex)
        {
        }

        public virtual T GetObject(int index = 0)
        {
            if (!_pools.ContainsKey(index))
            {
                Debug.LogError($"Pool index {index} does not exist!");
                return null;
            }

            return _pools[index].Get();
        }

        public virtual void ReturnObject(T item)
        {
            if (item == null)
            {
                Debug.LogError("Attempting to return null object to pool!");
                return;
            }

            if (!_poolIndices.TryGetValue(item, out int poolIndex))
            {
                Debug.LogError($"Object {item.name} was not created by this pool!");
                return;
            }

            if (_pools.TryGetValue(poolIndex, out var pool))
            {
                pool.Release(item);
            }
        }

        protected virtual void OnGet(T item)
        {
            if (item == null) return;

            switch (item)
            {
                case GameObject go:
                    go.SetActive(true);
                    break;
                case Component component:
                    component.gameObject.SetActive(true);
                    break;
            }

            if (_poolIndices.TryGetValue(item, out int poolIndex))
            {
                OnObjectRetrieved(item, poolIndex);
            }
        }

        protected virtual void OnRelease(T item)
        {
            if (item == null) return;

            switch (item)
            {
                case GameObject go:
                    go.SetActive(false);
                    break;
                case Component component:
                    component.gameObject.SetActive(false);
                    break;
            }

            if (_poolIndices.TryGetValue(item, out int poolIndex))
            {
                OnObjectReturned(item, poolIndex);
            }
        }

        private void HandleObjectDestruction(T item)
        {
            if (item == null) return;

            _poolIndices.Remove(item);

            switch (item)
            {
                case GameObject go:
                    Destroy(go);
                    break;
                case Component component:
                    Destroy(component.gameObject);
                    break;
            }

            OnPoolObjectDestroyed(item);
        }

        protected virtual void OnPoolObjectDestroyed(T item)
        {
        }

        protected virtual void OnObjectRetrieved(T item, int poolIndex)
        {
        }

        protected virtual void OnObjectReturned(T item, int poolIndex)
        {
        }

        protected virtual void OnDestroy()
        {
            foreach (var pool in _pools.Values)
            {
                if (pool is ObjectPool<T> objectPool)
                {
                    objectPool.Clear();
                }
            }

            _pools.Clear();
            _poolIndices.Clear();
        }
    }

    namespace PoolSystem
    {
        public interface IPoolBase
        {
            void InitializePools();
        }
    }
}