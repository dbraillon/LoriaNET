using System;
using System.Data.Entity;
using System.Data.SQLite;

namespace LoriaNET.Storage.Database
{
    /// <summary>
    /// This is where Loria stores more advanced configuration.
    /// </summary>
    public class LoriaDbContext : DbContext
    {
        static string ConnectionString => $@"Data Source={AppDomain.CurrentDomain.BaseDirectory}\loria.db;";

        public DbSet<PersonEntity> Persons { get; set; }

        static LoriaDbContext()
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<LoriaDbContext, Configuration>());
        }

        public LoriaDbContext() 
            : base(new SQLiteConnection(ConnectionString), true)
        {
        }
    }
}
