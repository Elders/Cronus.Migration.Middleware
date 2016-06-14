using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Elders.Cronus.Migration.Middleware.Tests.TestMigration;
using Elders.Cronus.Migration.Middleware.Tests.TestModel;
using Elders.Cronus.Migration.Middleware.Tests.TestModel.Bar;
using Elders.Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Elders.Cronus.Migration.Middleware.Tests.AggregateCommitMigrationMiddleware
{
    [Subject("AggregateCommitMigrationMiddleware")]
    public class When_migrating_with_aggregate_commit_middleware
    {
        Establish context = () =>
         {
             migration = new AddEventMigration();
             aggregateCommitMigrationMiddleware = new Elders.Cronus.Migration.Middleware.AggregateCommitMigrationMiddleware(migration);
             eventStore = new TestEventStore();
             player = new TestEventStorePlayer(eventStore);
             migrationOuput = new List<AggregateCommit>();


             fooId = new FooId("1234", "elders");
             aggregateCommitFoo = new AggregateCommit(fooId.RawId, "bc", 0, new List<IEvent>
                {
                    new TestCreateEventFoo(fooId),
                    new TestUpdateEventFoo(fooId, string.Empty)
                });

             var barId = new BarId("1234", "elders");
             aggregateCommitBar = new AggregateCommit(barId.RawId, "bc", 0, new List<IEvent>
                {
                    new TestCreateEventBar(barId)
                });

             eventStore.Append(aggregateCommitFoo);
             eventStore.Append(aggregateCommitBar);


         };

        Because of = () =>
        {
            foreach (var commit in player.LoadAggregateCommits())
            {
                migrationOuput.AddRange(aggregateCommitMigrationMiddleware.Run(commit));
            }
        };

        It the_migration_should_return_two_commits = () => migrationOuput.Count.ShouldEqual(2);
        It the_migration_should_add_new_event_to_right_commite = () => migrationOuput.Where(m => m.AggregateRootId.SequenceEqual(fooId.RawId)).Single().Events.Count.ShouldEqual(aggregateCommitFoo.Events.Count + 1);

        static Elders.Cronus.Migration.Middleware.AggregateCommitMigrationMiddleware aggregateCommitMigrationMiddleware;
        static IMigration<AggregateCommit, IEnumerable<AggregateCommit>> migration;
        static TestEventStore eventStore;
        static AggregateCommit aggregateCommitFoo;
        static AggregateCommit aggregateCommitBar;
        static List<AggregateCommit> migrationOuput;
        static IEventStorePlayer player;
        static FooId fooId;
    }
}
