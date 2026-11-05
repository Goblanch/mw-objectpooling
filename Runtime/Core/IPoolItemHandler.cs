namespace MiradorWorks.Pooling
{
    /// <summary>
    /// Knows what a pooled object is and how to manage it. The pool only decides
    /// when each method is called.
    /// </summary>
    /// <typeparam name="T">Pooled type</typeparam>
    public interface IPoolItemHandler<T> where T : class
    {
        /// <summary>
        /// Creates a new object. It must be returned already in its inactive state.
        /// </summary>
        T Create();

        /// <summary>
        /// Called when the object is handed out, after any preparation step has run.
        /// </summary>
        void OnGet(T item);

        /// <summary>
        /// Called when the object is given back to the pool.
        /// </summary>
        void OnRelease(T item);

        /// <summary>
        /// Permanently destroys the object.
        /// </summary>
        /// <param name="item"></param>
        void Destroy(T item);
    }
}