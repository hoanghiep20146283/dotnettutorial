using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CourseManagement.ErrorHandlers.ConfictNameException)
        {
            // Handle ConfictNameException here
            _logger.LogError("[Global Handler] ConfictNameException occurred: {0}", context.Exception.Message);

            // For example, you can return a custom error response
            context.Result = new ConflictObjectResult(context.Exception.Message);
            context.ExceptionHandled = true; 
        } else if (context.Exception is KeyNotFoundException)
        {
            _logger.LogError("[Global Handler] KeyNotFoundException occurred: {0}", context.Exception.Message);

            context.Result = new NotFoundResult();
            context.ExceptionHandled = true;
        }
        else {
            _logger.LogError("[Global Handler] Unknown Exception occurred: {0}", context.Exception.Message);

            context.Result = new UnprocessableEntityObjectResult("Unknown Errors. Please try again later.");
            context.ExceptionHandled = true;
        }
    }
}
