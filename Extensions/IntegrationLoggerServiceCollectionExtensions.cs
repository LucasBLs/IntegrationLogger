using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace IntegrationLogger.Extensions;
public static class IntegrationLoggerServiceCollectionExtensions
{
    public static IServiceCollection AddIntegrationLogger(this IServiceCollection services)
    {
        services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
        {
            var embeddedFileProvider = new EmbeddedFileProvider(typeof(IntegrationLoggerServiceCollectionExtensions).Assembly);
            options.FileProviders.Add(embeddedFileProvider);
        });

        return services;
    }
}