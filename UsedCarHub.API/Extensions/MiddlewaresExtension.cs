using UsedCarHub.API.Middlewares;

namespace UsedCarHub.API.Extensions
{
    public static class MiddlewaresExtension
    {
        public static IApplicationBuilder UseCookieProvider(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieProviderMiddleware>();
        }
    }
}