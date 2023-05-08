using IntegrationLogger.Configuration;
using IntegrationLogger.Data;
using IntegrationLogger.Enums;
using IntegrationLogger.Repositories.ApiGateway;
using IntegrationLogger.Repositories.Integration;
using IntegrationLogger.Repositories.Interfaces;
using IntegrationLogger.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddScoped(x => new RelationalIntegrationLogRepository(x.GetRequiredService<IntegrationLogContextBase>()));
        services.AddScoped(x => new RelationalIntegrationLogRepository(x.GetRequiredService<IntegrationLogContextSqlServer>()));
        services.AddScoped(x => new RelationalIntegrationLogRepository(x.GetRequiredService<IntegrationLogContextPostgreSQL>()));
        services.AddScoped(x => new RelationalIntegrationLogRepository(x.GetRequiredService<IntegrationLogContextOracle>()));

        services.AddScoped<IIntegrationLogRepository>(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<IntegrationLoggerConfiguration>();

            if (config.Provider == DatabaseProvider.MongoDB)
            {
                return serviceProvider.GetRequiredService<MongoDBIntegrationLogRepository>();
            }
            else
            {
                return serviceProvider.GetRequiredService<RelationalIntegrationLogRepository>();
            }
        });

        services.AddHostedService<MigrateDatabaseHostedService>();

        if (provider != DatabaseProvider.MongoDB)
        {
            switch (provider)
            {
                case DatabaseProvider.SqlServer:
                    ProviderDb.LoggerDatabaseProvider = DatabaseProvider.SqlServer;
                    services.AddDbContext<IntegrationLogContextSqlServer>((serviceProvider, options) =>
                    {
                        var config = serviceProvider.GetRequiredService<IntegrationLoggerConfiguration>();
                        options.UseSqlServer(config.ConnectionString);
                    });
                    break;
                case DatabaseProvider.PostgreSQL:
                    ProviderDb.LoggerDatabaseProvider = DatabaseProvider.PostgreSQL;
                    services.AddDbContext<IntegrationLogContextPostgreSQL>((serviceProvider, options) =>
                    {
                        var config = serviceProvider.GetRequiredService<IntegrationLoggerConfiguration>();
                        options.UseNpgsql(config.ConnectionString);
                    });
                    break;
                case DatabaseProvider.Oracle:
                    ProviderDb.LoggerDatabaseProvider = DatabaseProvider.Oracle;
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

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Integration Logger API", Version = "v1" });
            c.CustomSchemaIds(type => type.FullName);
        });

        services.AddIntegrationLogger();
        services.AddControllers();

        return services;
    }

    public static IApplicationBuilder UseIntegrationLogger(this IApplicationBuilder app)
    {
        app.UseRouting();

        // Adicione estas linhas para configurar o Swagger:
        app.UseSwagger(c => c.RouteTemplate = "integration-logger-swagger/{documentName}/swagger.json");
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/integration-logger-swagger/v1/swagger.json", "Integration Logger API v1");
            c.RoutePrefix = "integration-logger-swagger";
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
