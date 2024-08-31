using RestauracjaApi2.Exceptions;

namespace RestauracjaApi2.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex, ex.Message);    // exception logowany do pliku

                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ex.Message);     // status i info trafiające do klienta
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);    // exception logowany do pliku

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("something not yes");     // status i info trafiające do klienta
            }
        }
    }
}
