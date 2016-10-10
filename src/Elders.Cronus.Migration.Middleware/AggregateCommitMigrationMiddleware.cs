using System;
using Elders.Cronus.Middleware;
using Elders.Cronus.EventStore;
using System.Collections.Generic;
using System.Linq;
using Elders.Cronus.Migration.Middleware.Logging;

namespace Elders.Cronus.Migration.Middleware
{
    public class AggregateCommitMigrationMiddleware : GenericMigrationMiddleware<AggregateCommit, IEnumerable<AggregateCommit>>
    {
        static readonly ILog log = LogProvider.GetLogger(typeof(AggregateCommitMigrationMiddleware));

        public AggregateCommitMigrationMiddleware(IMigration<AggregateCommit, IEnumerable<AggregateCommit>> migration) : base(migration)
        {
        }

        protected override IEnumerable<AggregateCommit> Run(Execution<AggregateCommit, IEnumerable<AggregateCommit>> context)
        {
            var commit = context.Context;
            var newCommits = new List<AggregateCommit> { commit };
            try
            {
                if (migration.ShouldApply(commit))
                    newCommits = migration.Apply(commit).ToList();
            }
            catch (Exception ex)
            {
                log.ErrorException("Error while applying migration", ex);
            }

            foreach (var newCommit in newCommits)
                yield return newCommit;
        }
    }
}
