﻿namespace Elders.Cronus.Migration.Middleware
{
    public interface IMigration<in T, out V>
    {
        bool ShouldApply(T current);
        V Apply(T current);
    }

    public interface IMigration<T> : IMigration<T, T>
    { }
}
