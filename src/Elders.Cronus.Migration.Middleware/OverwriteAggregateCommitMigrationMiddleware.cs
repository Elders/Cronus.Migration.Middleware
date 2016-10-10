using System;
using Elders.Cronus.EventStore;
using Elders.Cronus.Middleware;
using Elders.Cronus.Migration.Middleware.Logging;

namespace Elders.Cronus.Migration.Middleware
{
    public class OverwriteAggregateCommitMigrationMiddleware : GenericMigrationMiddleware<AggregateCommit, AggregateCommit>
    {
        static readonly ILog log = LogProvider.GetLogger(typeof(OverwriteAggregateCommitMigrationMiddleware));

        public OverwriteAggregateCommitMigrationMiddleware(IMigration<AggregateCommit, AggregateCommit> migration) : base(migration)
        {
        }

        protected override AggregateCommit Run(Execution<AggregateCommit, AggregateCommit> context)
        {
            AggregateCommit result = context.Context;
            var commit = context.Context;
            try
            {
                if (migration.ShouldApply(commit))
                    result = migration.Apply(commit);
            }
            catch (Exception ex)
            {
                log.ErrorException("Error while applying migration", ex);
            }

            return result;
        }
    }
}
