using IntegrationLogger.Configuration;
using IntegrationLogger.Data;
using IntegrationLogger.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IntegrationLogger.Utils
{
    public class MigrateDatabaseHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public MigrateDatabaseHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var config = scope.ServiceProvider.GetRequiredService<IntegrationLoggerConfiguration>();
            switch (config.Provider)
            {
                case DatabaseProvider.SqlServer:
                    await MigrateAsync<IntegrationLogContextSqlServer>(scope, cancellationToken);
                    break;
                case DatabaseProvider.PostgreSQL:
                    await MigrateAsync<IntegrationLogContextPostgreSQL>(scope, cancellationToken);
                    break;
                case DatabaseProvider.Oracle:
                    await MigrateAsync<IntegrationLogContextOracle>(scope, cancellationToken);
                    break;
                default:
                    throw new ArgumentException($"Provedor de banco de dados não suportado: {config.Provider}");
            }
        }

        private static async Task MigrateAsync<T>(IServiceScope scope, CancellationToken cancellationToken) where T : IntegrationLogContextBase
        {
            var dbContext = scope.ServiceProvider.GetService<T>();
            if (dbContext != null)
            {
                var migrator = dbContext.Database.GetService<IMigrator>();
                await migrator.MigrateAsync(cancellationToken: cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
