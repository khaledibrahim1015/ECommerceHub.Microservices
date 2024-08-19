using Microsoft.AspNetCore.Builder;

namespace Common.Logging.Correlation;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder AddCorrelationIdMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<CorrelationIdMiddleware>();

}
