using Microsoft.AspNetCore.Diagnostics;
using ReactTreeTestTask.Server.Interfaces;
using System.Text.Json.Serialization;

namespace ReactTreeTestTask.Server
{
    class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, $"Exception occurred: {exception.Message}", exception.Message);
            
            string id = httpContext.TraceIdentifier;
            var details = new ExceptionResponse
            {
                Id = id,
            };

            if (exception is SecureException)
            {
                details.ExceptionType = "Secure";
                details.Data = new { message = exception.Message };
            } else
            {
                details.ExceptionType = "Exception";
                details.Data = new { message = $"Internal server error ID = {id}" };
            }
            var journalService = httpContext.RequestServices.CreateScope().ServiceProvider.GetService<IJournalService>();
            await journalService.Create(exception, httpContext);

            //httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
            return true;
        }

        class ExceptionResponse
        {
            [JsonPropertyName("type")]
            public string? ExceptionType { get; set; }
            public string Id { get; set; }
            public object? Data { get; set; }
        }
    }
}
