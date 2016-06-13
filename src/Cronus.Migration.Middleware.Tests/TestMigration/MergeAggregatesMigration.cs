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
        //readonly string targetAggregateFoo = "Foo".ToLowerInvariant();
        readonly string targetAggregateBar = "Bar".ToLowerInvariant();
        readonly Dictionary<IAggregateRootId, int> kvp;

        readonly IEventStore eventStore;

        public MergeAggregatesMigration(IEventStore eventStore)
        {
            if (ReferenceEquals(eventStore, null) == true) throw new System.ArgumentNullException(nameof(eventStore));
            this.eventStore = eventStore;

            kvp = new Dictionary<IAggregateRootId, int>();
        }

        private void LoadFromEventStore(IAggregateRootId rootId)
        {
            if (kvp.ContainsKey(rootId)) return;
            var count = eventStore.Load(rootId).Commits.Max(c => c.Revision);
            kvp.Add(rootId, count);
        }

        public IEnumerable<AggregateCommit> Apply(AggregateCommit current)
        {
            if (ShouldApply(current))
            {
                var tenantUrn = new TenantUrn(Encoding.UTF8.GetString(current.AggregateRootId).ToLowerInvariant());
                var fooId = new FooId(tenantUrn.Parts[3], tenantUrn.Tenant);
                LoadFromEventStore(fooId);
                kvp[fooId]++;

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
                var aggregateCommitFooBar = new AggregateCommit(fooId.RawId, current.BoundedContext, kvp[fooId], newFooEvents);
                yield return aggregateCommitFooBar;

            }
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
            var tenantUrn = new TenantUrn(Encoding.UTF8.GetString(current.AggregateRootId).ToLowerInvariant());
            string currentAggregateName = tenantUrn.Parts[2];

            if (currentAggregateName == targetAggregateBar)
                return true;

            return false;
        }
    }
}
