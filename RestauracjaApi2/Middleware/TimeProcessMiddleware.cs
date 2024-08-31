

using System.Diagnostics;

namespace RestauracjaApi2.Middleware
{
    public class TimeProcessMiddleware : IMiddleware
    {
        private readonly ILogger<TimeProcessMiddleware> logger;

        public TimeProcessMiddleware(ILogger<TimeProcessMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            await next.Invoke(context);

            stopwatch.Stop();
            var time = stopwatch.Elapsed;
            //if (time.Seconds > 4) 
            string message = $"Request {context.Request.Method} at {context.Request.Path} has {time.Milliseconds} [ms]";
            logger.LogInformation(message);
        }
    }
}
