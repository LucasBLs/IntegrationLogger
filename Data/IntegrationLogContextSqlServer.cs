using Microsoft.EntityFrameworkCore;

namespace IntegrationLogger.Data;
public class IntegrationLogContextSqlServer : IntegrationLogContextBase
{
    public IntegrationLogContextSqlServer(DbContextOptions<IntegrationLogContextSqlServer> options) : base(options)
    {
    }
}