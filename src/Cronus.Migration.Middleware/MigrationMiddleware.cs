using System;
using Elders.Cronus.MessageProcessingMiddleware;
using Elders.Cronus.Middleware;
using static Elders.Cronus.MessageProcessingMiddleware.MessageHandlerMiddleware;

namespace Cronus.Migration.Middleware
{
    public class MigrationMiddleware<T> : Middleware<MigrationManager<T>>
    {
        public MigrationMiddleware(MigrationManager<T> manager)
        {
            if (ReferenceEquals(manager, null) == true) throw new System.ArgumentNullException(nameof(manager));

        }
        protected override void Run(Execution<MigrationManager<T>> context)
        {
            throw new NotImplementedException();
        }
    }


}
