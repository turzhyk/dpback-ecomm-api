using System.Text.RegularExpressions;

namespace DPBack.API.Middleware;

public sealed class TrafficAnalyzer(RequestDelegate next, ILogger<TrafficAnalyzer> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if ((context.Request.Path == "/swagger/index.js") && HttpMethods.IsGet(context.Request.Method))
        {
            var userIp = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers.UserAgent.ToString();

            if (!IsBot(context.Request))
                logger.LogInformation("user ({ip}, {agent}) opened Swagger", userIp, userAgent);
        }

        await next(context);
    }

    private bool IsBot(HttpRequest request)
    {
        var userAgent = request.Headers.UserAgent.ToString();
        return string.IsNullOrWhiteSpace(userAgent) ||
               Regex.IsMatch(
                   userAgent,
                   @"bot|crawler|spider|headless|curl|wget|python|httpclient|scrapy|axios",
                   RegexOptions.IgnoreCase);
    }
}