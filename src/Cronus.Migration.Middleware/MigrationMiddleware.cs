using System;
using Elders.Cronus.MessageProcessingMiddleware;
using Elders.Cronus.Middleware;
using static Elders.Cronus.MessageProcessingMiddleware.MessageHandlerMiddleware;

namespace Cronus.Migration.Middleware
{
    public class MigrationMiddleware : Middleware<HandleContext>
    {
        Middleware<HandleContext> actualHandle;

        public MigrationMiddleware(Middleware<HandleContext> actualHandle)
        {
            this.actualHandle = actualHandle;
        }

        protected override void Run(Execution<HandleContext> context)
        {
            throw new NotImplementedException();
        }
    }


}
