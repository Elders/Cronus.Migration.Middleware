﻿using Cronus.Migration.Middleware.Tests.TestMigration;
using Cronus.Migration.Middleware.Tests.TestModel.Foo;
using Elders.Cronus.DomainModeling;
using Elders.Cronus.EventStore;
using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Cronus.Migration.Middleware.Tests.Migration
{
    [Subject("Migration")]
    public class When_adding_event_to_aggregateCommit_with_migration
    {
        Establish context = () =>
        {
            migration = new AddEventMigration();
            var id = new FooId("1234", "elders");
            aggregateCommitFoo = new List<AggregateCommit> { new AggregateCommit(id.RawId, "bc", 0, new List<IEvent> { new TestCreateEventFoo(id) }) };
        };

        Because of = () => migrationOuput = migration.Apply(aggregateCommitFoo).ToList();

        It the_migration_should_add_new_event = () => migrationOuput.Single().Events.Count.ShouldEqual(2);

        static IMigration<AggregateCommit> migration;
        static IList<AggregateCommit> aggregateCommitFoo;
        static IList<AggregateCommit> migrationOuput;
    }
}