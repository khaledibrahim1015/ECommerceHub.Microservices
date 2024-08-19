using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Common.Logging.Correlation;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string _correlationIdHeader = "X-Correlation-Id";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
    {

        var correclationId = GetCorrelationId(context, correlationIdGenerator);
        AddCorreclationHeader(context, correclationId);
        await _next(context);

    }


    private StringValues GetCorrelationId(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
    {

        if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
        {
            correlationIdGenerator.Set(correlationId);
            return correlationId;
        }
        else
        {
            return correlationIdGenerator.Get();
        }
    }

    private void AddCorreclationHeader(HttpContext context, StringValues correclationId)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add(_correlationIdHeader, new[] { correclationId.ToString() });
            return Task.CompletedTask;

        });
    }
}
