using Elders.Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elders.Cronus.Migration.Middleware.Tests.TestMigration
{
    public class RemoveEventMigration : IMigration<AggregateCommit, IEnumerable<AggregateCommit>>
    {
        readonly string targetAggregateName = "Foo".ToLowerInvariant();
        static readonly FooId id = new FooId("1234", "elders");
        static readonly string contractIdToRemove = typeof(TestUpdateEventFoo).GetContractId();

        public IEnumerable<AggregateCommit> Apply(AggregateCommit current)
        {
            if (ShouldApply(current))
            {
                var newEvents = new List<IEvent>(current.Events);
                newEvents.RemoveAll(x => x.GetType().GetContractId() == contractIdToRemove);
                var newAggregateCommit = new AggregateCommit(current.AggregateRootId, current.BoundedContext, current.Revision, newEvents);

                yield return newAggregateCommit;
            }

            else
                yield return current;
        }

        public bool ShouldApply(AggregateCommit current)
        {
            var tenantUrn = new TenantUrn(Encoding.UTF8.GetString(current.AggregateRootId).ToLowerInvariant());
            string currentAggregateName = tenantUrn.Parts[2];

            if (currentAggregateName == targetAggregateName)
                return true;

            return false;
        }
    }
}
