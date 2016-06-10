using Cronus.Migration.Middleware.Tests.TestMigration;
using Cronus.Migration.Middleware.Tests.TestModel.Bar;
using Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Machine.Specifications;
using System.Collections.Generic;

namespace Cronus.Migration.Middleware.Tests.Migration
{
    [Subject("Migration")]
    public class When_evalueting_if_migration_should_apply
    {
        Establish context = () =>
        {
            migration = new SimpleMigration();
            aggregateCommitFoo = new AggregateCommit(new FooId("1234", "elders").RawId, "bc", 0, new List<IEvent>());
            aggregateCommitBar = new AggregateCommit(new BarId("1234", "elders").RawId, "bc", 0, new List<IEvent>());
        };

        Because of = () => { };

        It the_evaluation_should_be_true = () => migration.ShouldApply(aggregateCommitFoo).ShouldBeTrue();
        It the_should_apply_should_be_false = () => migration.ShouldApply(aggregateCommitBar).ShouldBeFalse();


        static IMigration<AggregateCommit> migration;
        static AggregateCommit aggregateCommitFoo;
        static AggregateCommit aggregateCommitBar;
    }
}
