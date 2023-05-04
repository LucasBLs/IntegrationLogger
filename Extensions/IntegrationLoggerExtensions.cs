using IntegrationLogger.Configuration;
using IntegrationLogger.Data;
using IntegrationLogger.Enums;
using IntegrationLogger.Interfaces;
using IntegrationLogger.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace IntegrationLogger.Extensions;
public static class IntegrationLoggerExtensions
{
    public static IServiceCollection AddIntegrationLogger(
        this IServiceCollection services,
        string connectionString,
        DatabaseProvider provider,
        string? mongoDatabaseName = null,
        string? roles = null)
    {
        var migrationsDirectoryName = provider switch
        {
            DatabaseProvider.SqlServer => "MigrationSqlServer",
            DatabaseProvider.PostgreSQL => "MigrationPostgreSQL",
            DatabaseProvider.Oracle => "MigrationOracle",
            _ => throw new ArgumentException($"Provedor de banco de dados não suportado: {provider}")
        };

        var migrationsDirectory = $"Migrations/{migrationsDirectoryName}";

        services.AddScoped(_ => new IntegrationLoggerConfiguration
        {
            ConnectionString = connectionString,
            Provider = provider,
            MongoDatabaseName = mongoDatabaseName,
            Roles = roles,
            MigrationsDirectory = migrationsDirectory
        });

        services.AddScoped(x => new RoleBasedAuthorizationFilter(roles));
        services.AddScoped(x => IntegrationLogServiceFactory.CreateIntegrationLogService(x.GetRequiredService<IntegrationLoggerConfiguration>(), x));
        services.AddScoped<MongoDBIntegrationLogService>();
        services.AddScoped(x => new RelationalIntegrationLogService(x.GetRequiredService<IntegrationLogContextBase>()));
        services.AddScoped(x => new RelationalIntegrationLogService(x.GetRequiredService<IntegrationLogContextSqlServer>()));
        services.AddScoped(x => new RelationalIntegrationLogService(x.GetRequiredService<IntegrationLogContextPostgreSQL>()));
        services.AddScoped(x => new RelationalIntegrationLogService(x.GetRequiredService<IntegrationLogContextOracle>()));

        services.AddScoped<IIntegrationLogQueryable>(x =>
        {
            if (x.GetRequiredService<IntegrationLoggerConfiguration>().Provider == DatabaseProvider.MongoDB)
            {
                return x.GetRequiredService<MongoDBIntegrationLogService>();
            }
            return x.GetRequiredService<RelationalIntegrationLogService>();
        });

        services.AddHostedService<MigrateDatabaseHostedService>();

        if (provider != DatabaseProvider.MongoDB)
        {
            switch (provider)
            {
                case DatabaseProvider.SqlServer:
                    ProviderDb.databaseProvider = DatabaseProvider.SqlServer;
                    services.AddDbContext<IntegrationLogContextSqlServer>((serviceProvider, options) =>
                    {
                        var config = serviceProvider.GetRequiredService<IntegrationLoggerConfiguration>();
                        options.UseSqlServer(config.ConnectionString);
                    });
                    break;
                case DatabaseProvider.PostgreSQL:
                    ProviderDb.databaseProvider = DatabaseProvider.PostgreSQL;
                    services.AddDbContext<IntegrationLogContextPostgreSQL>((serviceProvider, options) =>
                    {
                        var config = serviceProvider.GetRequiredService<IntegrationLoggerConfiguration>();
                        options.UseNpgsql(config.ConnectionString);
                    });
                    break;
                case DatabaseProvider.Oracle:
                    ProviderDb.databaseProvider = DatabaseProvider.Oracle;
                    services.AddDbContext<IntegrationLogContextOracle>((serviceProvider, options) =>
                    {
                        var config = serviceProvider.GetRequiredService<IntegrationLoggerConfiguration>();
                        options.UseOracle(config.ConnectionString);
                    });
                    break;
                default:
                    throw new ArgumentException($"Provedor de banco de dados não suportado: {provider}");
            }
        }

        services.AddIntegrationLogger();
        services.AddControllersWithViews().AddRazorRuntimeCompilation();

        return services;
    }

    public static IApplicationBuilder UseIntegrationLogger(this IApplicationBuilder app)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly(), "IntegrationLogger.Views.wwwroot"),
            RequestPath = ""
        });
        return app;
    }
}
