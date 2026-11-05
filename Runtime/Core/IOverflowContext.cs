namespace MiradorWorks.Pooling
{
    public interface IOverflowContext<T> where T : class
    {
        /// <summary>
        /// Objects currently handed out.
        /// </summary>
        int CountActive { get; }

        /// <summary>
        /// Maximum number of active objects. Always greater than zero here.
        /// </summary>
        int MaxActive { get; }

        /// <summary>
        /// Takes an inactive object, or creates a new one if there are none.
        /// The pool activates the returned object after the policy returns it.
        /// </summary>
        T TakeInactiveOrCreate();

        /// <summary>
        /// Releases the oldest active object (calling <see cref="IPoolItemHandler{T}.OnRelease"/>)
        /// and returns it so it can be handed out again. The pool activates it again
        /// after the policy returns it.
        /// </summary>
        T TakeOldestActive();
    }
}