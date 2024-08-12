using Ordering.Api.Midddleware;

namespace Ordering.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalExceptionHanding(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandling>();
            return app;
        }
    }
}
