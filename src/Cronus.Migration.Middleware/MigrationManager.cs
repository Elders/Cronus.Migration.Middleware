using Elders.Cronus.EventStore;
using System.Collections.Generic;
using System;

namespace Cronus.Migration.Middleware
{
    public class MigrationManager<T>
    {
        IList<IMigration<T>> RegisteredMigrations;

        public MigrationManager()
        {
            RegisteredMigrations = new List<IMigration<T>>();
        }

        public void Register(IMigration<T> migration)
        {
            if (ReferenceEquals(migration, null) == true) throw new System.ArgumentNullException(nameof(migration));
            RegisteredMigrations.Add(migration);
        }

        //public IEnumerable<V> RunMigration(T current)
        //{
        //    List<V> next = new List<V>();
        //    foreach (var migration in RegisteredMigrations)
        //    {
        //        if (migration.ShouldApply(current))
        //        {
        //            next.AddRange(migration.Apply(current));
        //        }
        //    }
        //}
    }



    public class GGMigration : IMigration<int>
    {
        public IEnumerable<int> Apply(int current)
        {
            throw new NotImplementedException();
        }

        public bool ShouldApply(int current)
        {
            throw new NotImplementedException();
        }
    }




    public class GG
    {
        public GG()
        {
            var migrationManager = new MigrationManager<int>();

            migrationManager.Register(new GGMigration());
        }
    }
}