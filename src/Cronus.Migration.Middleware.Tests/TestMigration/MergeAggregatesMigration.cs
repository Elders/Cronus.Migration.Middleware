using Cronus.Migration.Middleware.Tests.TestModel.Bar;
using Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Cronus.Migration.Middleware.Tests.TestModel.FooBar;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cronus.Migration.Middleware.Tests.TestMigration
{
    public class MergeAggregatesMigration : IMigration<AggregateCommit, IEnumerable<AggregateCommit>>
    {
        readonly string targetAggregateBar = "Bar".ToLowerInvariant();
        readonly Dictionary<IAggregateRootId, int> aggregateMaxRevision;
        readonly IEventStore eventStore;

        public MergeAggregatesMigration(IEventStore eventStore)
        {
            if (ReferenceEquals(eventStore, null) == true) throw new System.ArgumentNullException(nameof(eventStore));
            this.eventStore = eventStore;

            aggregateMaxRevision = new Dictionary<IAggregateRootId, int>();
        }

        private void LoadFromEventStore(IAggregateRootId rootId)
        {
            if (aggregateMaxRevision.ContainsKey(rootId)) return;

            var stream = eventStore.Load(rootId);
            if (ReferenceEquals(stream, null) == true)
            {
                aggregateMaxRevision.Add(rootId, 0);
            }
            else
            {
                var count = stream.Commits.Max(c => c.Revision);
                aggregateMaxRevision.Add(rootId, count);
            }
        }

        public IEnumerable<AggregateCommit> Apply(AggregateCommit current)
        {
            if (ShouldApply(current))
            {
                var tenantUrn = new TenantUrn(Encoding.UTF8.GetString(current.AggregateRootId).ToLowerInvariant());
                var fooId = new FooId(tenantUrn.Parts[3], tenantUrn.Tenant);
                LoadFromEventStore(fooId);
                aggregateMaxRevision[fooId]++;

                var newFooEvents = new List<IEvent>();
                foreach (IEvent @event in current.Events)
                {
                    if (@event.GetType() == typeof(TestCreateEventBar))
                    {
                        newFooEvents.Add(new TestCreateEventFoo(fooId));
                    }
                    else if (@event.GetType() == typeof(TestUpdateEventBar))
                    {
                        var theEvent = @event as TestUpdateEventBar;
                        newFooEvents.Add(new TestUpdateEventFoo(fooId, theEvent.UpdatedFieldValue));
                    }
                }
                var aggregateCommitFooBar = new AggregateCommit(fooId.RawId, current.BoundedContext, aggregateMaxRevision[fooId], newFooEvents);
                yield return aggregateCommitFooBar;

            }
            else
                yield return current;
        }

        public bool ShouldApply(AggregateCommit current)
        {
            var tenantUrn = new TenantUrn(Encoding.UTF8.GetString(current.AggregateRootId).ToLowerInvariant());
            string currentAggregateName = tenantUrn.Parts[2];

            if (currentAggregateName == targetAggregateBar)
                return true;

            return false;
        }
    }
}
