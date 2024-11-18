using System;
using System.Collections.Generic;

namespace EmptyCommon.Pools
{
    public abstract class BaseComplexPool<Key, Value>
    {
        private readonly Dictionary<Key, BasePool<Value>> PoolsTable = new();

        public void AddPoolObject(Key key, Value value)
        {
            PoolsTable[key] = CreatePool(value);
        }

        public Value Spawn(Key key)
        {
            if (!PoolsTable.ContainsKey(key))
                throw new NullReferenceException($"[{this.GetType().Name}] Не содержит пулл с ключем - {key}");

            return PoolsTable[key].Spawn();
        }

        public void Despawn(Key key, Value value)
        {
            if (PoolsTable.TryGetValue(key, out var pool))
            {
                pool.Despawn(value);
            }
            else
            {
                DestoryObject(value);
            }
        }

        protected abstract BasePool<Value> CreatePool(Value value);
        protected abstract void DestoryObject(Value value);

        protected bool IsContaned(Key key) => PoolsTable.ContainsKey(key);
    }
}
