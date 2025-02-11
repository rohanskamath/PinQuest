using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace dotnetcorebackend.Infrastructure.Middleware
{
    public class UserActivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserActivityMiddleware> _logger;
        private readonly string logDirectory = "Logs";
        private readonly string logFilePath;

        public UserActivityMiddleware(RequestDelegate next, ILogger<UserActivityMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            logFilePath = Path.Combine(logDirectory, "user_activity.log");

            // Ensure log directory and file exist
            EnsureLogFileExists();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var request = context.Request;
                if (request.Path.ToString().Contains("/api/register") || request.Path.ToString().Contains("/api/login"))
                {
                    context.Request.EnableBuffering();
                    using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
                    {
                        var requestBody = await reader.ReadToEndAsync();
                        request.Body.Position = 0; // Reset position for further processing

                        string email = ExtractEmail(requestBody);
                        Console.WriteLine(requestBody);
                        string action = request.Path.ToString().Contains("/api/register") ? "User Registered" : "User Logged In";

                        // Log the action
                        LogUserAction(action, email);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Logging Error: {ex.Message}");
            }

            await _next(context);
        }

        private void LogUserAction(string action, string email)
        {
            string logEntry = $"{DateTime.UtcNow} - Action: {action} - Email: {email}{Environment.NewLine}";

            try
            {
                File.AppendAllText(logFilePath, logEntry); // Append log
                _logger.LogInformation(logEntry); // Also log using ILogger
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to write log: {ex.Message}");
            }
        }

        private void EnsureLogFileExists()
        {
            try
            {
                // Create directory if it doesn't exist
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Create log file if it doesn't exist
                if (!File.Exists(logFilePath))
                {
                    using (File.Create(logFilePath)) { } // Create and close file
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create log directory or file: {ex.Message}");
            }
        }

        private string ExtractEmail(string requestBody)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestBody);
            if (data != null && data.ContainsKey("email"))
            {
                return data["email"];
            }
            return "Anonymous";
        }
    }


}
