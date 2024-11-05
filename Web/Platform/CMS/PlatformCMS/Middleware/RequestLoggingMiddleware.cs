using MI.Dapper.Data.Constant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly string LogDirectoryPath;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, IHostingEnvironment env)
    {
        _next = next;
        _logger = logger;
        LogDirectoryPath = Path.Combine(env.WebRootPath, "logs"); // Set path to wwwroot/logs

        if (!Directory.Exists(LogDirectoryPath))
        {
            Directory.CreateDirectory(LogDirectoryPath);
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Request.Path;

        // Only log if the endpoint starts with /api/
        if (endpoint.StartsWithSegments("/api"))
        {
            var requestTime = DateTime.UtcNow;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            context.Request.EnableBuffering();
            string requestBody = string.Empty;

            if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
            {
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            if (context.Request.HasFormContentType)
            {
                foreach (var file in context.Request.Form.Files)
                {
                    requestBody += $"[File: {file.FileName}, Size: {file.Length} bytes]";
                }
            }

            // Extract Bearer token from the Authorization header
            string bearerToken = string.Empty;
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authorizationHeader = context.Request.Headers["Authorization"].ToString();
                if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    bearerToken = authorizationHeader.Substring("Bearer ".Length).Trim();
                }
            }

            await _next(context);

            // Log request details, including the Bearer token
            LogRequestToFile(requestTime, ipAddress, endpoint, requestBody, bearerToken);
        }
        else
        {
            // If the endpoint does not start with /api/, simply continue to the next middleware
            await _next(context);
        }
    }

    private void LogRequestToFile(DateTime requestTime, string ipAddress, string endpoint, string requestBody, string bearerToken)
    {
        try
        {
            var logFileName = Path.Combine(LogDirectoryPath, $"log_{DateTime.UtcNow:yyyy-MM-dd}.log");

            // Check if the file exceeds a certain size (e.g., 10 MB)
            const long maxFileSize = 10 * 1024 * 1024; // 10 MB in bytes
            if (File.Exists(logFileName) && new FileInfo(logFileName).Length > maxFileSize)
            {
                // Create a new file by appending a timestamp or counter
                logFileName = Path.Combine(LogDirectoryPath, $"log_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.log");
            }

            var logEntry = new StringBuilder();
            logEntry.AppendLine("==== Request Log Entry ====");
            logEntry.AppendLine($"Timestamp: {requestTime}");
            logEntry.AppendLine($"IP Address: {ipAddress}");
            logEntry.AppendLine($"Bearer Token: {bearerToken}");
            logEntry.AppendLine($"Endpoint: {endpoint}");
            logEntry.AppendLine($"Request Body: {requestBody}");
            logEntry.AppendLine("===========================");

            File.AppendAllText(logFileName, logEntry.ToString());
        }
        catch (Exception ex)
        {
            var errorLogFileName = Path.Combine(LogDirectoryPath, $"log_{DateTime.UtcNow:yyyy-MM-dd}.log");
            var errorLogEntry = new StringBuilder();

            errorLogEntry.AppendLine("==== Log Error ====");
            errorLogEntry.AppendLine($"Timestamp: {DateTime.UtcNow}");
            errorLogEntry.AppendLine($"Error Message: {ex.Message}");
            errorLogEntry.AppendLine($"Stack Trace: {ex.StackTrace}");
            errorLogEntry.AppendLine("===================");

            File.AppendAllText(errorLogFileName, errorLogEntry.ToString());
        }
    }


}
