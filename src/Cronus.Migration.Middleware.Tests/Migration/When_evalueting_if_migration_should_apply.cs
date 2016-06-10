using Cronus.Migration.Middleware.Tests.TestMigration;
using Cronus.Migration.Middleware.Tests.TestModel.Bar;
using Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Cronus.Migration.Middleware.Tests.Migration
{
    [Subject("Migration")]
    public class When_evalueting_if_migration_should_apply
    {
        Establish context = () =>
        {
            migration = new SimpleMigration();
            var fooId = new FooId("1234", "elders");
            var barId = new BarId("1234", "elders");
            aggregateCommitFoo = new List<AggregateCommit> { new AggregateCommit(fooId.RawId, "bc", 0, new List<IEvent>()) };
            aggregateCommitBar = new List<AggregateCommit> { new AggregateCommit(barId.RawId, "bc", 0, new List<IEvent>()) };
        };

        Because of = () => { };

        It the_evaluation_should_be_true = () => migration.ShouldApply(aggregateCommitFoo.First()).ShouldBeTrue();
        It the_should_apply_should_be_false = () => migration.ShouldApply(aggregateCommitBar.First()).ShouldBeFalse();


        static IMigration<AggregateCommit> migration;
        static IList<AggregateCommit> aggregateCommitFoo;
        static IList<AggregateCommit> aggregateCommitBar;
    }
}
