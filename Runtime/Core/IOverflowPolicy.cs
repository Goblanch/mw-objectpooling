namespace MiradorWorks.Pooling
{
    /// <summary>
    /// Decides what happens when an object is requested and the pool has reaches 
    /// irs active limit.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOverflowPolicy<T> where T : class
    {
        /// <summary>
        /// Resolves an overflowing request.
        /// </summary>
        /// <param name="context">Operations the policy is allowed to use.</param>
        /// <returns>The object to hand out, <c>null<c/> to reject the request.</returns>
        T Resolve(IOverflowContext<T> context);
    }
}