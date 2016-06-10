using Elders.Cronus.EventStore;
using System.Collections.Generic;
using System.Linq;
using Elders.Cronus.DomainModeling;

namespace Cronus.Migration.Middleware.Tests.TestModel
{
    public class TestEventStore : IEventStore
    {
        IList<AggregateCommit> storage;

        public TestEventStore()
        {
            storage = new List<AggregateCommit>();
        }

        public void Append(AggregateCommit aggregateCommit)
        {
            storage.Add(aggregateCommit);
        }

        public EventStream Load(IAggregateRootId aggregateId)
        {
            var es = new EventStream(storage.Where(x => x.AggregateRootId == aggregateId.RawId).ToList());
            return es;
        }
    }
}
