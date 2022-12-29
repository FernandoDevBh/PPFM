using Constants;
using System.Net;
using System.Text.Json;
using Application.Errors;
using ValidationException = Application.Errors.ValidationException;

namespace API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlingMiddleware> logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception, logger);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlingMiddleware> logger)
    {
        object? errors = null;
        switch (exception)
        {
            case RestException re:
                logger.LogError(exception, ErrorTypes.REST_ERROR);
                errors = re.Erros;
                context.Response.StatusCode = (int)re.Code;
                break;
            case ValidationException ve:
                logger.LogError(exception, ErrorTypes.VALIDATION_ERRORS);
                errors = new
                {
                    ve.Title,
                    ve.Message,
                    ve.ErrorsDictionary
                };
                context.Response.StatusCode = (int)StatusCodes.Status422UnprocessableEntity;
                break;
            case Exception e:
                logger.LogError(exception, ErrorTypes.SERVER_ERROR);
                errors = string.IsNullOrWhiteSpace(exception.Message) ? "Error" : exception.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.ContentType = ContentTypes.JSON;

        if (errors != null)
        {
            var result = JsonSerializer.Serialize(new
            {
                errors
            });
            await context.Response.WriteAsync(result);
        }
    }
}
