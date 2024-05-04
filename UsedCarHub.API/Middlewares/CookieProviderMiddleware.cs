namespace UsedCarHub.API.Middlewares
{
    public class CookieProviderMiddleware
    {
        private readonly RequestDelegate _next;

        public CookieProviderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["usedCarHubId"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Append("Authorization", "Bearer " + token);
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Append("X-Xss-Protection", "1");
                context.Response.Headers.Append("X-Frame-Options", "DENY");
            }

            await _next(context);
        }
    }
}