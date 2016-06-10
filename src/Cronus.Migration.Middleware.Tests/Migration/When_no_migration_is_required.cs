using Cronus.Migration.Middleware.Tests.TestMigration;
using Cronus.Migration.Middleware.Tests.TestModel.Bar;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Cronus.Migration.Middleware.Tests.Migration
{
    [Subject("Migration")]
    public class When_no_migration_is_required
    {
        Establish context = () =>
        {
            var barId = new BarId("1234", "elders");
            migration = new SimpleMigration();
            aggregateCommitBar = new List<AggregateCommit> { new AggregateCommit(barId.RawId, "bc", 0, new List<IEvent> { new TestCreateEventBar(barId) }) };
        };

        Because of = () => migrationOuput = migration.Apply(aggregateCommitBar).ToList();

        It the_migration_output_should_not_be_null = () => migrationOuput.ShouldNotBeNull();
        It the_migration_should_not_change_aggregateCommit_count = () => migrationOuput.Count.ShouldEqual(1);
        It the_migration_output_should_be_same_as_the_input = () => migrationOuput.ShouldContainOnly(aggregateCommitBar);


        static IMigration<AggregateCommit> migration;
        static IList<AggregateCommit> aggregateCommitBar;
        static IList<AggregateCommit> migrationOuput;
    }
}