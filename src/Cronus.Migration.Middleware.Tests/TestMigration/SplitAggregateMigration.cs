﻿using Cronus.Migration.Middleware.Tests.TestModel.Bar;
using Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Cronus.Migration.Middleware.Tests.TestModel.FooBar;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using System.Collections.Generic;
using System.Text;

namespace Cronus.Migration.Middleware.Tests.TestMigration
{
    public class SplitAggregateMigration : IMigration<AggregateCommit>
    {
        readonly string targetAggregateName = "FooBar".ToLowerInvariant();
        static readonly FooBarId id = new FooBarId("1234", "elders");

        public IEnumerable<AggregateCommit> Apply(IEnumerable<AggregateCommit> items)
        {
            foreach (AggregateCommit current in items)
            {
                if (ShouldApply(current))
                {
                    var fooId = new FooId("1234", "elders");
                    var newFooEvents = new List<IEvent>();
                    foreach (IEvent @event in current.Events)
                    {
                        if (@event.GetType() == typeof(TestCreateEventFooBar))
                        {
                            newFooEvents.Add(new TestCreateEventFoo(fooId));
                        }
                        else if (@event.GetType() == typeof(TestUpdateEventFooBar))
                        {
                            var theEvent = @event as TestUpdateEventFooBar;
                            newFooEvents.Add(new TestUpdateEventFoo(fooId, theEvent.UpdatedFieldValue));
                        }
                    }
                    var aggregateCommitFoo = new AggregateCommit(fooId.RawId, "bc", 0, newFooEvents);
                    yield return aggregateCommitFoo;

                    var barId = new BarId("5432", "elders");
                    var newBarEvents = new List<IEvent>();
                    foreach (IEvent @event in current.Events)
                    {
                        if (@event.GetType() == typeof(TestCreateEventFooBar))
                        {
                            newBarEvents.Add(new TestCreateEventBar(barId));
                        }
                        else if (@event.GetType() == typeof(TestUpdateEventFooBar))
                        {
                            var theEvent = @event as TestUpdateEventFooBar;
                            newBarEvents.Add(new TestUpdateEventBar(barId, theEvent.UpdatedFieldValue));
                        }
                    }
                    var aggregateCommitBar = new AggregateCommit(barId.RawId, "bc", 0, newFooEvents);

                    yield return aggregateCommitBar;

                }
                else
                    yield return current;

            }
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
