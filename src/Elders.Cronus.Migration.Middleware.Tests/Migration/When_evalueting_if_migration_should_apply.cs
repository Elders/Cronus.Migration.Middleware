﻿using Elders.Cronus.Migration.Middleware.Tests.TestMigration;
using Elders.Cronus.Migration.Middleware.Tests.TestModel.Bar;
using Elders.Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Machine.Specifications;
using System.Collections.Generic;

namespace Elders.Cronus.Migration.Middleware.Tests.Migration
{
    [Subject("Migration")]
    public class When_evalueting_if_migration_should_apply
    {
        Establish context = () =>
        {
            migration = new SimpleMigration();
            var fooId = new FooId("1234", "elders");
            var barId = new BarId("1234", "elders");
            aggregateCommitFoo = new AggregateCommit(fooId.RawId, "bc", 0, new List<IEvent>());
            aggregateCommitBar = new AggregateCommit(barId.RawId, "bc", 0, new List<IEvent>());
        };

        Because of = () => { };

        It the_evaluation_should_be_true = () => migration.ShouldApply(aggregateCommitFoo).ShouldBeTrue();
        It the_should_apply_should_be_false = () => migration.ShouldApply(aggregateCommitBar).ShouldBeFalse();

        static IMigration<AggregateCommit, IEnumerable<AggregateCommit>> migration;
        static AggregateCommit aggregateCommitFoo;
        static AggregateCommit aggregateCommitBar;
    }
}
