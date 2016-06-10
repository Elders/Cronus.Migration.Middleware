using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cronus.Migration.Middleware.Tests.TestModel.FooBar
{
    [DataContract(Name = "58c3f873-73dc-4592-a9f4-370b4bb23395")]
    public class TestCreateEventFooBar : IEvent
    {
        public TestCreateEventFooBar(FooBarId id)
        {
            Id = id;
        }

        [DataMember(Order = 1)]
        public FooBarId Id { get; set; }
    }
}
