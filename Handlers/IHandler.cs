using IntegrationLogger.Commands.Interfaces;

namespace IntegrationLogger.Handlers;
public interface IHandler<T> where T : ICommand
{
    Task<ICommandResult> HandleAsync(T command);
}