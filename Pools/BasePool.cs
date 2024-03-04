using System.Collections.Generic;

namespace EmptyCommon.Pools
{
    public abstract class BasePool<T>
    {
        private readonly Queue<T> Pool = new Queue<T>();

        public BasePool(T prototype)
        {
            Prototype = prototype;
        }

        protected T Prototype { get; }

        public T Spawn()
        {
            var @object = (Pool.Count > 0)
                                ? Pool.Dequeue()
                                : Create();

            OnSpawn(@object);

            return @object;
        }

        public void Despawn(T @object)
        {
            OnDespawn(@object);
            Pool.Enqueue(@object);
        }

        protected abstract T Create();
        protected virtual void OnSpawn(T spawnedObject) { }
        protected virtual void OnDespawn(T despawnedObject) { }
    }
}
