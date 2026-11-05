namespace MiradorWorks.Pooling
{
    /// <summary>
    /// A pool of reusable objects. Objects taken with <see cref="cref="Get"/> are "active
    /// until they are given back with <see cref="cref="Release"/>, then they become "inactive"
    /// and can be handed out again.  
    /// </summary>
    /// <typeparam name="T">Pooled type</typeparam>
    public interface IPool<T> where T : class
    {
        /// <summary>Objects currently handed out and not yet released.</summary>
        int CountActive { get; }

        /// <summary>Objects stored in the pool, ready to be handed out.</summary>
        int CountInactive { get; }

        /// <summary>Total objects owned by the pool: <c>CountActive + CountInactive</c>.</summary>
        int CountAll { get; }

        /// <summary>
        /// Takes an object from the pool, creating one if needed.
        /// Returns <c>null<c/> if the overflow policy rejects the request.
        /// </summary>
        T Get();

        /// <summary>
        /// Takes an object from the pool, creating one if needed.
        /// Returns <c>false<c/> if the overflow policy rejects the request.
        /// </summary>
        /// <param name="item"></param>
        bool TryGet(out T item);

        /// <summary>
        /// Gives an active object back to the pool
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The object is not active in this pool (already released or owned by another pool).
        /// </exception>
        void Release(T item);

        /// <summary>
        /// Creates inactive object until <see cref="CountAll"/> is at least <paramref name="count"/>.
        /// </summary>
        void Prewarm(int count);

        /// <summary>
        /// Releases every active object.
        /// </summary>
        void ReleaseAll();

        /// <summary>
        /// Destroys every inactive object. Active objects are not affected,
        /// because someone is still using them.
        /// </summary>
        void Clear();
    }
}