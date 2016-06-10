using Cronus.Migration.Middleware.Tests.TestMigration;
using Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Cronus.Migration.Middleware.Tests.Migration
{
    [Subject("Migration")]
    public class When_removing_event_from_aggregateCommit_with_migration
    {
        Establish context = () =>
        {
            var id = new FooId("1234", "elders");
            migration = new SimpleRemoveEventMigration();
            aggregateCommitFoo = new AggregateCommit(id.RawId, "bc", 0, new List<IEvent> { new TestCreateEventFoo(id), new TestUpdateEventFoo(id, string.Empty) });
        };

        Because of = () => migrationOuput = migration.Apply(aggregateCommitFoo).ToList();

        It the_migration_should_add_new_event = () => migrationOuput.Single().Events.Count.ShouldEqual(1);

        static IMigration<AggregateCommit> migration;
        static AggregateCommit aggregateCommitFoo;
        static IList<AggregateCommit> migrationOuput;
    }
}