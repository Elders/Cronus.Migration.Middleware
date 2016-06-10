using Cronus.Migration.Middleware.Tests.TestModel.Bar;
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
                    var fooId = new FooId("2345", "elders");
                    var aggregateCommitFoo = new AggregateCommit(fooId.RawId, "bc", 0, new List<IEvent> {
                    new TestCreateEventFoo(fooId),
                    new TestUpdateEventFoo(fooId, string.Empty)
                });
                    yield return aggregateCommitFoo;

                    var barId = new BarId("5432", "elders");
                    var aggregateCommitBar = new AggregateCommit(barId.RawId, "bc", 0, new List<IEvent>
                {
                    new TestCreateEventBar(barId),
                    new TestUpdateEventBar(barId, string.Empty)
                });

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
