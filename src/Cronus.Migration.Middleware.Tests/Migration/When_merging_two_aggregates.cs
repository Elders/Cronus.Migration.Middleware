using Cronus.Migration.Middleware.Tests.TestMigration;
using Cronus.Migration.Middleware.Tests.TestModel;
using Cronus.Migration.Middleware.Tests.TestModel.Bar;
using Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Cronus.Migration.Middleware.Tests.TestModel.FooBar;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Cronus.Migration.Middleware.Tests.Migration
{
    [Subject("Migration")]
    public class When_merging_two_aggregates
    {
        Establish context = () =>
        {
            var eventStore = new TestEventStore();

            migration = new MergeAggregatesMigration(eventStore);
            migrationOuput = new List<AggregateCommit>();
            var fooId = new FooId("1234", "elders");
            aggregateCommitFoo = new List<AggregateCommit>
            {
                new AggregateCommit(fooId.RawId, "bc", 0, new List<IEvent>
                {
                    new TestCreateEventFoo(fooId),
                    new TestUpdateEventFoo(fooId, string.Empty)
                })
            };

            foreach (var commit in aggregateCommitFoo)
            {
                eventStore.Append(commit);
            }


            var barId = new BarId("1234", "elders");
            aggregateCommitBar = new List<AggregateCommit>
            {
                new AggregateCommit(barId.RawId, "bc", 0, new List<IEvent>
                {
                    new TestCreateEventBar(barId)
                })
            };
        };

        Because of = () =>
        {
            migrationOuput.AddRange(migration.Apply(aggregateCommitBar).ToList());
        };

        It the_migration_should_return_two_aggegateCommits = () => migrationOuput.Count.ShouldEqual(1);
        It the_migration_should_contain_correnct_number_of_events = () => migrationOuput.SelectMany(x => x.Events).Count().ShouldEqual(1);
        It the_migration_should_contain_only_events_from_new_aggregate =
            () => migrationOuput.Select(x => x.Events.Select(e => e.GetType().GetContractId())).ShouldContain(contracts);

        static IMigration<AggregateCommit> migration;
        static IList<AggregateCommit> aggregateCommitFoo;
        static IList<AggregateCommit> aggregateCommitBar;
        static List<AggregateCommit> migrationOuput;

        static List<string> contracts = new List<string>
        {
            typeof(TestCreateEventFoo).GetContractId()
        };
    }
}