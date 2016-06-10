using System;
using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;

namespace Cronus.Migration.Middleware.Tests.TestModel.Foo
{
    [DataContract(Name = "0b745bdc-0a45-4220-9dfd-804652504594")]
    public class FooId : StringTenantId
    {
        FooId() { }

        public FooId(StringTenantId id) : base(id, "Foo") { }

        public FooId(string id, string tenant) : base(id, "Foo", tenant) { }
    }
}