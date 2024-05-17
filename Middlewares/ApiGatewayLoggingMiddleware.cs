using Amazon.Auth.AccessControlPolicy;
using IntegrationLogger.Enums;
using IntegrationLogger.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web;

public class ApiGatewayLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _ignoredPaths = new List<string>
    {
        "/_blazor",
        "/_blazor/negotiate",
        "/gateway-log",
        "/",
        "/_framework/blazor.server.js",
        "/_content/MudBlazor/MudBlazor.min.css",
        "/_content/MudBlazor/MudBlazor.min.js",
        "/integration-logger-swagger/v1/swagger.json",
        "/integration-logger-swagger/index.html",
        "/swagger",
        "/swagger/index.html",
        "/favicon.ico",
        "/wwwroot/favicon.ico",
        "/integration-logger-swagger/",
        "/integration-logger-swagger/swagger-ui.css",
        "/integration-logger-swagger/swagger-ui-standalone-preset.js",
        "/integration-logger-swagger/swagger-ui-bundle.js",
        "/integration-logger-swagger/favicon-32x32.png",
        "/dashboard",
        "/dashboard/stats",
        "/dashboard/css182042560001",
        "/dashboard/css-dark18201324172007",
        "/dashboard/recurring",
        "/dashboard/fonts/glyphicons-halflings-regular/woff2",
        "/logs",
        "/Order"
    };

    public ApiGatewayLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IApiGatewayLogRepository repository)
    {
        if (_ignoredPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
        {
            await _next(context);
            return;
        }

        var watch = new Stopwatch();
        watch.Start();

        var originalResponseBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            var projectName = context.Items["ProjectName"] as string ?? "Project Not Found";
            var requestContent = await FormatRequest(context.Request);

            watch.Stop();
            var gatewayLog = repository.AddGatewayLog(projectName, context.Request.Path, context.Request.Method, $"{context.Connection.RemoteIpAddress}", context.Response.StatusCode, watch.ElapsedMilliseconds);
            repository.AddGatewayDetail(gatewayLog, DetailType.Request, "Request Received", requestContent);

            var statusCode = context.Response.StatusCode;
            if (context.Response.Headers.ContainsKey("X-StatusCode"))
            {
                int.TryParse(context.Response.Headers["X-StatusCode"], out statusCode);
            }

            var responseContent = await FormatResponse(context.Response);
            repository.AddGatewayDetail(gatewayLog, DetailType.Response, "Response Sent", responseContent, statusCode);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
        }
        finally
        {
            responseBody.Seek(0, SeekOrigin.Begin); // Resetar a posição do stream antes de copiar
            await responseBody.CopyToAsync(originalResponseBodyStream);
        }
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();

        string bodyAsText;
        using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
        {
            bodyAsText = await reader.ReadToEndAsync();
            request.Body.Position = 0; // Resetar a posição do stream do corpo da requisição para permitir a leitura por outros middlewares
        }

        var queryString = request.QueryString.HasValue ? Uri.UnescapeDataString($"{request.QueryString.Value}") : "";
        var url = $"{request.Scheme}://{request.Host}{request.Path} {queryString}";

        return $"{url} {bodyAsText}";
    }

    private async Task<object> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        string text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin); // Resetar a posição do stream do corpo da resposta para permitir a leitura por outros middlewares

        if (response.ContentType != null && response.ContentType.ToLower().Contains("application/json"))
        {
            try
            {
                return JObject.Parse(text);
            }
            catch (JsonReaderException)
            {
                // Log your exception or handle it accordingly
                return text;
            }
        }
        else
        {
            return text;
        }
    }
}
