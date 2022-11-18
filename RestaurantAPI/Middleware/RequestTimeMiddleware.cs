using System.Diagnostics;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private Stopwatch _stopwatch;
        private readonly ILogger<RequestTimeMiddleware> _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _stopwatch = new Stopwatch();
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();

            var elapsedTime = _stopwatch.ElapsedMilliseconds;

            if(elapsedTime  > 1)
            {
                var message = $"Time of {context.Request.Method} in {context.Request.Path} was longer than 4 seconds ({elapsedTime}ms.)";
                _logger.LogInformation(message);
            }
        }
    }
}
