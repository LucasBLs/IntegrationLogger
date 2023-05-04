using Microsoft.EntityFrameworkCore;

namespace IntegrationLogger.Data;
public class IntegrationLogContextOracle : IntegrationLogContextBase
{
    public IntegrationLogContextOracle(DbContextOptions<IntegrationLogContextOracle> options) : base(options)
    {
    }
}
