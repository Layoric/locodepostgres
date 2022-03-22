using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

[assembly: HostingStartup(typeof(locodepostgres.ConfigureDbPostgres))]

namespace locodepostgres
{
    // Example Data Model
    public class MyTable
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ConfigureDbPostgres : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) => builder
            .ConfigureServices((context, services) =>
            {
                PostgreSqlDialect.Instance.NamingStrategy = new MyPostgreSqlNamingStrategy();
                services.AddSingleton<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                    context.Configuration.GetConnectionString("DefaultConnection")
                    ?? "Server=localhost;User Id=postgres;Password=postgres;Database=locodetesting;Pooling=true;MinPoolSize=0;MaxPoolSize=200",
                    PostgreSqlDialect.Provider));
            })
             //Create non-existing Table and add Seed Data Example
            .ConfigureAppHost(appHost => {
                using var db = appHost.Resolve<IDbConnectionFactory>().Open();
                if (db.CreateTableIfNotExists<MyTable>())
                {
                    db.Insert(new MyTable { Name = "Seed Data for new MyTable" });
                }
            });
    }
    
    public class MyPostgreSqlNamingStrategy : OrmLiteNamingStrategyBase
    {
        public override string GetTableName(string name)
        {
            return name;
        }

        public override string GetColumnName(string name) 
        {
            return name;
        }

        public override string GetSchemaName(string name)
        {
            return name;
        }
    }
}
