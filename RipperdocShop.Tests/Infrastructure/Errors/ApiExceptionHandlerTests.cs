using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

    [Fact]
    public async Task TryHandleAsync_Logs_Only_Sanitized_Context_For_Unhandled_Exception()
    {
        var logger = new CapturingLogger<ApiExceptionHandler>();
        var handler = new ApiExceptionHandler(logger);
        var context = new DefaultHttpContext
        {
            Request =
            {
                Path = "/api/test\r\nforged-entry"
            },
            Response =
            {
                Body = new MemoryStream()
            }
        };
        var exception = new Exception("attacker-controlled\r\nexception-message");

        await handler.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.Equal(LogLevel.Error, logger.LastLevel);
        Assert.Null(logger.LastException);
        Assert.DoesNotContain("\r", logger.LastMessage);
        Assert.DoesNotContain("\n", logger.LastMessage);
        Assert.Contains("/api/testforged-entry", logger.LastMessage);
        Assert.DoesNotContain("attacker-controlled", logger.LastMessage);
    }

    private static Exception CreateException(Type exceptionType, string message)
    {
        if (exceptionType == typeof(ArgumentOutOfRangeException))
            return new ArgumentOutOfRangeException("value", message);

        return (Exception)Activator.CreateInstance(exceptionType, message)!;
    }
}


internal sealed class CapturingLogger<T> : ILogger<T>
{
    public LogLevel LastLevel { get; private set; }

    public string LastMessage { get; private set; } = string.Empty;

    public Exception? LastException { get; private set; }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        LastLevel = logLevel;
        LastException = exception;
        LastMessage = formatter(state, exception);
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}
