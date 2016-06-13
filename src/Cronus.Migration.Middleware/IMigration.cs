using System.Collections.Generic;

namespace Cronus.Migration.Middleware
{
    public interface IMigration<in T, out V>
    {
        bool ShouldApply(T current);
        V Apply(T current);
        V Apply(IEnumerable<T> items);
    }

    public interface IMigration<T> : IMigration<T, T>
    { }
}
