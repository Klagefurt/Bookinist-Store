using Bookinist.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace Bookinist_Store.Data
{
    public static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration Configuration) => services
            .AddDbContext<BookinistDB>(opt =>
            {
                var type = Configuration["Type"];
                switch(type)
                {
                    case null: throw new InvalidOperationException("Db type is not defined!");
                    default: throw new InvalidOperationException($"Connection type {type} is not supported!");

                    case "PostgreSQL":
                        var psqlConnString = Configuration.GetConnectionString("PostgreSQL");
                        if (string.IsNullOrEmpty(psqlConnString))
                            throw new InvalidOperationException("PostgreSQL connection string is not defined.");
                        opt.UseSqlServer(psqlConnString);
                        break;
                    case "MSSQL":
                        var mssqlConnString = Configuration.GetConnectionString("MSSQL");
                        if (string.IsNullOrEmpty(mssqlConnString))
                            throw new InvalidOperationException("MSSQL connection string is not defined.");
                        opt.UseSqlServer(mssqlConnString);
                        break;
                    case "SQLite":
                        var sqliteConnString = Configuration.GetConnectionString("SQLite");
                        if (string.IsNullOrEmpty(sqliteConnString))
                            throw new InvalidOperationException("SQLite connection string is not defined.");
                        opt.UseSqlite(sqliteConnString);
                        break;
                    case "InMemory":
                        opt.UseInMemoryDatabase("Bookinist.db");
                        break;
                }
            })
        ;
    }
}
