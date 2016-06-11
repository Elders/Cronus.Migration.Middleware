using Cronus.Migration.Middleware.Tests.TestModel.Bar;
using Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Cronus.Migration.Middleware.Tests.TestModel.FooBar;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using System.Collections.Generic;
using System.Text;

namespace Cronus.Migration.Middleware.Tests.TestMigration
{
    public class ProduceNewAggregateMigration : IMigration<AggregateCommit>
    {
        readonly string targetAggregateFoo = "Foo".ToLowerInvariant();
        readonly string targetAggregateBar = "Bar".ToLowerInvariant();
        static readonly FooBarId id = new FooBarId("1234", "elders");

        public IEnumerable<AggregateCommit> Apply(IEnumerable<AggregateCommit> items)
        {
            foreach (AggregateCommit current in items)
            {
                if (ShouldApply(current))
                {
                    var tenantUrn = new TenantUrn(Encoding.UTF8.GetString(current.AggregateRootId).ToLowerInvariant());
                    string currentAggregateName = tenantUrn.Parts[2];

                    if (currentAggregateName == targetAggregateFoo)
                    {
                        var fooBarId = new FooBarId("1234", "elders");
                        var newFooBarEvents = new List<IEvent>();
                        foreach (IEvent @event in current.Events)
                        {
                            if (@event.GetType() == typeof(TestCreateEventFoo))
                            {
                                newFooBarEvents.Add(new TestCreateEventFooBar(fooBarId));
                            }
                            else if (@event.GetType() == typeof(TestUpdateEventFoo))
                            {
                                var theEvent = @event as TestUpdateEventFoo;
                                newFooBarEvents.Add(new TestUpdateEventFooBar(fooBarId, theEvent.UpdatedFieldValue));
                            }
                        }
                        var aggregateCommitFooBar = new AggregateCommit(fooBarId.RawId, current.BoundedContext, current.Revision, newFooBarEvents);
                        yield return aggregateCommitFooBar;
                    }
                    else
                    {
                        var fooBarId = new FooBarId("1234", "elders");
                        var newFooBarEvents = new List<IEvent>();
                        foreach (IEvent @event in current.Events)
                        {
                            if (@event.GetType() == typeof(TestCreateEventBar))
                            {
                                newFooBarEvents.Add(new TestCreateEventFooBar(fooBarId));
                            }
                            else if (@event.GetType() == typeof(TestUpdateEventBar))
                            {
                                var theEvent = @event as TestUpdateEventBar;
                                newFooBarEvents.Add(new TestUpdateEventFooBar(fooBarId, theEvent.UpdatedFieldValue));
                            }
                        }
                        var aggregateCommitFooBar = new AggregateCommit(fooBarId.RawId, current.BoundedContext, current.Revision, newFooBarEvents);
                        yield return aggregateCommitFooBar;
                    }
                }
                else
                    yield return current;
            }
        }

        public bool ShouldApply(AggregateCommit current)
        {
            var tenantUrn = new TenantUrn(Encoding.UTF8.GetString(current.AggregateRootId).ToLowerInvariant());
            string currentAggregateName = tenantUrn.Parts[2];

            if (currentAggregateName == targetAggregateFoo || currentAggregateName == targetAggregateBar)
                return true;

            return false;
        }
    }
}
