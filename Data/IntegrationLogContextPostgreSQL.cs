using Microsoft.EntityFrameworkCore;

namespace IntegrationLogger.Data;
public class IntegrationLogContextPostgreSQL : IntegrationLogContextBase
{
    public IntegrationLogContextPostgreSQL(DbContextOptions<IntegrationLogContextPostgreSQL> options) : base(options)
    {
    }
}