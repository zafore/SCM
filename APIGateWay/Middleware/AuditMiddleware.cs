using APIGateWay.Services;
using System.Security.Claims;
using System.Text;

namespace APIGateWay.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;

        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuditService auditService)
        {
            var startTime = DateTime.UtcNow;
            var request = context.Request;
            var response = context.Response;

            // Skip audit for certain paths
            if (ShouldSkipAudit(request.Path))
            {
                await _next(context);
                return;
            }

            // Capture request data
            var requestBody = await CaptureRequestBody(request);
            var originalResponseBody = response.Body;

            using var responseBodyStream = new MemoryStream();
            response.Body = responseBodyStream;

            try
            {
                await _next(context);
            }
            finally
            {
                // Capture response data
                var responseBody = await CaptureResponseBody(responseBodyStream);
                await responseBodyStream.CopyToAsync(originalResponseBody);
                response.Body = originalResponseBody;

                // Log audit
                await LogAuditAsync(context, auditService, startTime, requestBody, responseBody);
            }
        }

        private bool ShouldSkipAudit(PathString path)
        {
            var skipPaths = new[]
            {
                "/swagger",
                "/health",
                "/favicon.ico",
                "/_framework",
                "/css",
                "/js",
                "/images"
            };

            return skipPaths.Any(skipPath => path.StartsWithSegments(skipPath));
        }

        private async Task<string> CaptureRequestBody(HttpRequest request)
        {
            if (request.ContentLength > 0 && request.ContentType?.Contains("application/json") == true)
            {
                request.EnableBuffering();
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return body;
            }
            return string.Empty;
        }

        private async Task<string> CaptureResponseBody(MemoryStream responseBodyStream)
        {
            responseBodyStream.Position = 0;
            using var reader = new StreamReader(responseBodyStream, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            responseBodyStream.Position = 0;
            return body;
        }

        private async Task LogAuditAsync(HttpContext context, IAuditService auditService, DateTime startTime, string requestBody, string responseBody)
        {
            try
            {
                var request = context.Request;
                var response = context.Response;
                var user = context.User;

                var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                var userName = user?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
                var userEmail = user?.FindFirst(ClaimTypes.Email)?.Value;
                var role = user?.FindFirst(ClaimTypes.Role)?.Value;

                var action = GetActionFromHttpMethod(request.Method);
                var entityType = GetEntityTypeFromPath(request.Path);
                var entityId = GetEntityIdFromPath(request.Path);
                var microservice = GetMicroserviceFromPath(request.Path);

                var auditRequest = new APIGateWay.Models.AuditLogRequest
                {
                    UserId = userId,
                    UserName = userName,
                    UserEmail = userEmail,
                    Action = action,
                    EntityType = entityType,
                    EntityId = entityId,
                    HttpMethod = request.Method,
                    Endpoint = request.Path,
                    Microservice = microservice,
                    IpAddress = GetClientIpAddress(context),
                    UserAgent = request.Headers["User-Agent"].FirstOrDefault(),
                    StatusCode = response.StatusCode.ToString(),
                    RequestData = TruncateData(requestBody, 1000),
                    ResponseData = TruncateData(responseBody, 1000),
                    SessionId = context.Session?.Id,
                    Role = role,
                    Duration = DateTime.UtcNow - startTime,
                    AdditionalInfo = $"Request processed in {DateTime.UtcNow - startTime}ms"
                };

                // Add error message if status code indicates error
                if (response.StatusCode >= 400)
                {
                    auditRequest.ErrorMessage = $"HTTP {response.StatusCode} error";
                }

                await auditService.LogAsync(auditRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in audit middleware");
            }
        }

        private string GetActionFromHttpMethod(string method)
        {
            return method.ToUpper() switch
            {
                "GET" => "VIEW",
                "POST" => "CREATE",
                "PUT" => "UPDATE",
                "PATCH" => "UPDATE",
                "DELETE" => "DELETE",
                _ => "UNKNOWN"
            };
        }

        private string GetEntityTypeFromPath(PathString path)
        {
            var pathSegments = path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
            
            if (pathSegments.Length >= 2)
            {
                var entityType = pathSegments[1].ToLower();
                return entityType switch
                {
                    "identity" => "User",
                    "admin" => "Admin",
                    "suppliers" => "Supplier",
                    "offers" => "Offer",
                    "inventory" => "Inventory",
                    "orders" => "Order",
                    "payments" => "Payment",
                    "accounting" => "Accounting",
                    "lookup" => "Lookup",
                    _ => "Unknown"
                };
            }

            return "System";
        }

        private string? GetEntityIdFromPath(PathString path)
        {
            var pathSegments = path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
            
            // Look for numeric ID in path segments
            foreach (var segment in pathSegments)
            {
                if (int.TryParse(segment, out _))
                {
                    return segment;
                }
            }

            return null;
        }

        private string? GetMicroserviceFromPath(PathString path)
        {
            var pathValue = path.Value.ToLower();
            
            if (pathValue.Contains("/api/identity"))
                return "IdentityMicroservice";
            if (pathValue.Contains("/api/admin"))
                return "AdminMicroservice";
            if (pathValue.Contains("/api/suppliers"))
                return "Suppliers.Api";
            if (pathValue.Contains("/api/offers"))
                return "Suppliers.Api";
            if (pathValue.Contains("/api/lookup"))
                return "Suppliers.Api";
            if (pathValue.Contains("/api/inventory"))
                return "InventoryMicroservice";
            if (pathValue.Contains("/api/orders"))
                return "OrderMicroservice";
            if (pathValue.Contains("/api/payments"))
                return "Payments.Api";
            if (pathValue.Contains("/api/accounting"))
                return "Accounting.Api";

            return "APIGateWay";
        }

        private string GetClientIpAddress(HttpContext context)
        {
            var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            }
            
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = context.Connection.RemoteIpAddress?.ToString();
            }

            return ipAddress ?? "Unknown";
        }

        private string TruncateData(string data, int maxLength)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            if (data.Length <= maxLength)
                return data;

            return data.Substring(0, maxLength) + "...";
        }
    }
}
