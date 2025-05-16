namespace ElVision.Middleware
{
    public class RedirectRootMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectRootMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the request path is "/" and method is GET
            if (context.Request.Path == "/" && context.Request.Method == HttpMethods.Get)
            {
                // Redirect to "/dashboard"
                context.Response.Redirect("/dashboard");
                return;
            }

            await _next(context); // Proceed to the next middleware
        }
    }
}
