using Microsoft.AspNetCore.Diagnostics;

namespace PALM.BatchInterfaceTools.API.Helpers.Handlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
        {
            if (exception is AggregateException)
            {
                // Your response object
                var error = new { message = exception.Data[Constants.GeneralConstants.AggregateExceptionErrorMessage] };
                //httpContext.Response.Headers[Constants.GeneralConstants.AggregateExceptionErrorMessage] = exception.Data[Constants.GeneralConstants.AggregateExceptionErrorMessage]?.ToString();
                await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            }
            else
            {
                // Your response object
                var error = new { message = exception.Message };
                await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            }

            return true;
        }
    }
}
