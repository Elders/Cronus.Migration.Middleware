using System;
using Elders.Cronus.Middleware;
using Elders.Cronus.Migration.Middleware.Logging;

namespace Elders.Cronus.Migration.Middleware
{
    public class GenericMigrationMiddleware<T, V> : Middleware<T, V>
    {
        static readonly ILog log = LogProvider.GetLogger(typeof(GenericMigrationMiddleware<T, V>));

        protected readonly IMigration<T, V> migration;

        public GenericMigrationMiddleware(IMigration<T, V> migration)
        {
            if (ReferenceEquals(migration, null) == true) throw new ArgumentNullException(nameof(migration));
            this.migration = migration;
        }

        protected override V Run(Execution<T, V> context)
        {
            V result = default(V);
            var commit = context.Context;
            try
            {
                if (migration.ShouldApply((T)commit))
                    result = migration.Apply((T)commit);
            }
            catch (Exception ex)
            {
                log.ErrorException("Error while applying migration", ex);
            }

            return result;
        }
    }
}
