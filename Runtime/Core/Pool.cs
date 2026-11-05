using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using MiradorWorks.Pooling.Internal;

namespace MiradorWorks.Pooling
{
    /// <summary>
    /// Default <see cref="IPool{T}"/> implementation. Plain C#, no Unity dependencies.
    /// After prewarming, getting and releaseing objects does not allocate memory.
    /// </summary>
    /// <typeparam name="T">Pooled type.</typeparam>
    public sealed class Pool<T> : IPool<T> where T : class
    {
        private readonly IPoolItemHandler<T> _handler;

        // Inactive objects, used as a stack (push and pop at the end).
        private readonly List<T> _inactive;

        // Active objects, oldest first.
        private readonly LinkedList<T> _active = new LinkedList<T>();

        // One node per object owned by the pool, created once and reused.
        // A node is linked into _active while its object is active.
        private readonly Dictionary<T, LinkedListNode<T>> _nodes;

        public Pool(IPoolItemHandler<T> handler, int initialCapacity = 0)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), initialCapacity, "Capacity cannot be negative.");

            _handler = handler;
            _inactive = new List<T>(initialCapacity);
            _nodes = new Dictionary<T, LinkedListNode<T>>(initialCapacity, IdentityComparer<T>.Instance);
        }

        #region IPool

        public int CountActive => _active.Count;

        public int CountInactive => _inactive.Count;

        public int CountAll => _active.Count + _inactive.Count;

        public void Clear()
        {
            // Remove each item before destorying it, so the pool stays consistent
            // even if the handler throws halfway through.
            for (int i = _inactive.Count - 1; i >= 0; i--)
            {
                T item = _inactive[i];
                _inactive.RemoveAt(i);
                _nodes.Remove(item);
                _handler.Destroy(item);
            }
        }

        public T Get()
        {
            TryGet(out T item);
            return item;
        }

        public bool TryGet(out T item)
        {
            item = TakeInactiveOrCreate();
            Activate(item);
            return true;
        }

        public void Prewarm(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be negative.");

            while (CountAll < count)
                _inactive.Add(CreateItem());
        }

        public void Release(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (!_nodes.TryGetValue(item, out LinkedListNode<T> node) || node.List != _active)
                throw new InvalidOperationException(
                    "The item is not ative in this pool. It was already released or belongs to another pool.");

            _active.Remove(node);
            _handler.OnRelease(item);
            _inactive.Add(item);
        }

        public void ReleaseAll()
        {
            while (_active.First != null)
                Release(_active.First.Value);
        }

        #endregion

        private T TakeInactiveOrCreate()
        {
            int last = _inactive.Count - 1;
            if (last < 0)
                return CreateItem();

            T item = _inactive[last];
            _inactive.RemoveAt(last);
            return item;
        }

        private T CreateItem()
        {
            T item = _handler.Create();

            if (item == null)
                throw new InvalidOperationException($"{_handler.GetType().Name}.Create() returned null.");
            if (_nodes.ContainsKey(item))
                throw new InvalidOperationException(
                    $"{_handler.GetType().Name}.Create() returned an item that already belongs to this pool.");


            _nodes.Add(item, new LinkedListNode<T>(item));
            return item;
        }

        private void Activate(T item)
        {
            _active.AddLast(_nodes[item]);
            _handler.OnGet(item);
        }
    }
}