using IntegrationLogger.Data;

namespace IntegrationLogger.Interfaces;
public interface IIntegrationLogRelationalQueryable
{
    IntegrationLogContextBase GetLogContext();
}