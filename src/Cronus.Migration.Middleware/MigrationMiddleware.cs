using System;
using Elders.Cronus.MessageProcessingMiddleware;
using Elders.Cronus.Middleware;
using static Elders.Cronus.MessageProcessingMiddleware.MessageHandlerMiddleware;
using Elders.Cronus.EventStore;
using System.Collections.Generic;
using System.Linq;

namespace Cronus.Migration.Middleware
{
    public class AggregateCommitMigrationMiddleware : Middleware<AggregateCommit, IEnumerable<AggregateCommit>>
    {
        readonly IMigration<AggregateCommit, IEnumerable<AggregateCommit>> migration;

        public AggregateCommitMigrationMiddleware(IMigration<AggregateCommit, IEnumerable<AggregateCommit>> migration)
        {
            if (ReferenceEquals(migration, null) == true) throw new System.ArgumentNullException(nameof(migration));
            this.migration = migration;
        }

        protected override IEnumerable<AggregateCommit> Run(Execution<AggregateCommit, IEnumerable<AggregateCommit>> context)
        {
            var commit = context.Context;
            var newCommits = new List<AggregateCommit> { commit };

            if (migration.ShouldApply(commit))
                newCommits = migration.Apply(commit).ToList();

            foreach (var newCommit in newCommits)
                yield return newCommit;
        }
    }
}
