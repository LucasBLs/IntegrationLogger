using IntegrationLogger.Enums;
using IntegrationLogger.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        "/swagger"
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

        var originalResponseBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        var projectName = context.Items["ProjectName"] as string ?? "Project Not Found";
        var requestContent = await FormatRequest(context.Request);

        var gatewayLog = repository.AddGatewayLog(projectName, context.Request.Path, context.Request.Method, context.Connection.RemoteIpAddress?.ToString(), context.Response.StatusCode, 0);

        repository.AddGatewayDetail(gatewayLog, DetailType.Request, "Request Received", requestContent);

        var statusCode = context.Response.StatusCode;
        if (context.Response.Headers.ContainsKey("X-StatusCode"))
        {
            int.TryParse(context.Response.Headers["X-StatusCode"], out statusCode);
        }

        var responseContent = await FormatResponse(context.Response);

        repository.AddGatewayDetail(gatewayLog, DetailType.Response, "Response Sent", responseContent, statusCode);
        await responseBody.CopyToAsync(originalResponseBodyStream);
    }


    private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();
        var body = request.Body;
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        await request.Body.ReadAsync(buffer, 0, buffer.Length);
        var bodyAsText = System.Text.Encoding.UTF8.GetString(buffer);
        request.Body = body;
        return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
    }

    private async Task<object> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        string text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

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
