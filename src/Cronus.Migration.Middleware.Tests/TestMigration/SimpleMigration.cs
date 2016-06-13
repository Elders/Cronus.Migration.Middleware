using Elders.Cronus.EventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cronus.Migration.Middleware.Tests.TestMigration
{
    public class SimpleMigration : IMigration<AggregateCommit>
    {
        readonly string targetAggregateName = "Foo".ToLowerInvariant();

        public IEnumerable<AggregateCommit> Apply(AggregateCommit current)
        {
            if (ShouldApply(current))
                throw new NotImplementedException();
            else
                yield return current;
        }

        public IEnumerable<AggregateCommit> Apply(IEnumerable<AggregateCommit> items)
        {
            foreach (AggregateCommit current in items)
            {
                var result = Apply(current).ToList();
                foreach (var item in result)
                {
                    yield return item;
                }
            }
        }

        public bool ShouldApply(AggregateCommit current)
        {
            string currentAggregateName = Encoding.UTF8.GetString(current.AggregateRootId).Split(':')[2].ToLowerInvariant();

            if (currentAggregateName == targetAggregateName)
                return true;

            return false;
        }
    }
}
