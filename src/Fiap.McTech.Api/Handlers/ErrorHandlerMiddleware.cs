using Fiap.McTech.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.McTech.Api.Handlers
{
    internal class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly Dictionary<Type, int> _statusCodeMappings = new()
        {
            { typeof(EntityNotFoundException), StatusCodes.Status404NotFound },
            { typeof(PaymentRequiredException), StatusCodes.Status402PaymentRequired },
        };

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (McTechException ex)
            {
                var status = _statusCodeMappings.GetValueOrDefault(ex.GetType(), StatusCodes.Status400BadRequest);
                await DoReturn(context, ex.Message, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                await DoReturn(context, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task DoReturn(HttpContext context, string message, int status)
        {
            var problemDetails = new ProblemDetails { Status = status, Detail = message };
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
