using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MiradorWorks.Pooling.Internal
{
    /// <summary>
    /// Compares objects by reference only, ignoring any Equals/GetHashCode overrides.
    /// </summary>
    internal sealed class IdentityComparer<T> : IEqualityComparer<T> where T : class
    {
        public static readonly IdentityComparer<T> Instance = new IdentityComparer<T>();

        private IdentityComparer() { }

        public bool Equals(T x, T y) => ReferenceEquals(x, y);

        public int GetHashCode(T obj) => RuntimeHelpers.GetHashCode(obj);
    }
}