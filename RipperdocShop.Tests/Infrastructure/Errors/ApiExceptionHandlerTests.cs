using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using RipperdocShop.Api.Infrastructure.Errors;

namespace RipperdocShop.Tests.Infrastructure.Errors;

public class ApiExceptionHandlerTests
{
    [Theory]
    [InlineData(typeof(ArgumentException), StatusCodes.Status400BadRequest, "Invalid request")]
    [InlineData(typeof(ArgumentOutOfRangeException), StatusCodes.Status400BadRequest, "Invalid request")]
    [InlineData(typeof(InvalidOperationException), StatusCodes.Status400BadRequest, "Invalid operation")]
    [InlineData(typeof(Exception), StatusCodes.Status500InternalServerError, "Unexpected error")]
    public async Task TryHandleAsync_Maps_Exception_To_ProblemDetails(
        Type exceptionType,
        int expectedStatus,
        string expectedTitle)
    {
        var handler = new ApiExceptionHandler(NullLogger<ApiExceptionHandler>.Instance);
        var context = new DefaultHttpContext
        {
            Request =
            {
                Path = "/api/test"
            },
            Response =
            {
                Body = new MemoryStream()
            }
        };
        var exception = CreateException(exceptionType, "Something broke.");

        var handled = await handler.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(expectedStatus, context.Response.StatusCode);
        Assert.StartsWith("application/problem+json", context.Response.ContentType);

        context.Response.Body.Position = 0;
        var problem = await JsonSerializer.DeserializeAsync<ProblemDetails>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(problem);
        Assert.Equal(expectedStatus, problem.Status);
        Assert.Equal(expectedTitle, problem.Title);
        Assert.Contains("Something broke.", problem.Detail);
        Assert.Equal("/api/test", problem.Instance);
    }

    private static Exception CreateException(Type exceptionType, string message)
    {
        if (exceptionType == typeof(ArgumentOutOfRangeException))
            return new ArgumentOutOfRangeException("value", message);

        return (Exception)Activator.CreateInstance(exceptionType, message)!;
    }
}
