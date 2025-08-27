using APIGateWay.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace APIGateWay.Services
{
    public class AuditService : IAuditService
    {
        private readonly ILogger<AuditService> _logger;
        private readonly List<AuditLog> _auditLogs = new(); // In-memory storage for demo
        private readonly object _lock = new object();

        public AuditService(ILogger<AuditService> logger)
        {
            _logger = logger;
        }

        public async Task LogAsync(AuditLogRequest auditRequest)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    UserId = auditRequest.UserId,
                    UserName = auditRequest.UserName,
                    UserEmail = auditRequest.UserEmail,
                    Action = auditRequest.Action,
                    EntityType = auditRequest.EntityType,
                    EntityId = auditRequest.EntityId,
                    EntityName = auditRequest.EntityName,
                    HttpMethod = auditRequest.HttpMethod,
                    Endpoint = auditRequest.Endpoint,
                    Microservice = auditRequest.Microservice,
                    IpAddress = auditRequest.IpAddress,
                    UserAgent = auditRequest.UserAgent,
                    StatusCode = auditRequest.StatusCode,
                    RequestData = auditRequest.RequestData,
                    ResponseData = auditRequest.ResponseData,
                    ErrorMessage = auditRequest.ErrorMessage,
                    Changes = auditRequest.Changes,
                    SessionId = auditRequest.SessionId,
                    Role = auditRequest.Role,
                    Duration = auditRequest.Duration,
                    AdditionalInfo = auditRequest.AdditionalInfo,
                    Timestamp = DateTime.UtcNow
                };

                lock (_lock)
                {
                    _auditLogs.Add(auditLog);
                }

                _logger.LogInformation("Audit logged: {Action} on {EntityType} by {UserId}", 
                    auditRequest.Action, auditRequest.EntityType, auditRequest.UserId);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging audit: {Action} on {EntityType}", 
                    auditRequest.Action, auditRequest.EntityType);
            }
        }

        public async Task LogLoginAsync(string userId, string userName, string userEmail, string ipAddress, string userAgent, string? role = null)
        {
            var auditRequest = new AuditLogRequest
            {
                UserId = userId,
                UserName = userName,
                UserEmail = userEmail,
                Action = "LOGIN",
                EntityType = "User",
                EntityId = userId,
                EntityName = userName,
                HttpMethod = "POST",
                Endpoint = "/api/identity/login",
                Microservice = "IdentityMicroservice",
                IpAddress = ipAddress,
                UserAgent = userAgent,
                StatusCode = "200",
                Role = role,
                AdditionalInfo = "User successfully logged in"
            };

            await LogAsync(auditRequest);
        }

        public async Task LogLogoutAsync(string userId, string userName, string ipAddress)
        {
            var auditRequest = new AuditLogRequest
            {
                UserId = userId,
                UserName = userName,
                Action = "LOGOUT",
                EntityType = "User",
                EntityId = userId,
                EntityName = userName,
                HttpMethod = "POST",
                Endpoint = "/api/identity/logout",
                Microservice = "IdentityMicroservice",
                IpAddress = ipAddress,
                StatusCode = "200",
                AdditionalInfo = "User logged out"
            };

            await LogAsync(auditRequest);
        }

        public async Task LogActionAsync(string userId, string userName, string action, string entityType, string? entityId = null, string? entityName = null, string? changes = null)
        {
            var auditRequest = new AuditLogRequest
            {
                UserId = userId,
                UserName = userName,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                EntityName = entityName,
                Changes = changes,
                AdditionalInfo = $"{action} operation on {entityType}"
            };

            await LogAsync(auditRequest);
        }

        public async Task LogErrorAsync(string userId, string userName, string action, string entityType, string errorMessage, string? entityId = null)
        {
            var auditRequest = new AuditLogRequest
            {
                UserId = userId,
                UserName = userName,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                ErrorMessage = errorMessage,
                StatusCode = "500",
                AdditionalInfo = $"Error during {action} operation on {entityType}"
            };

            await LogAsync(auditRequest);
        }

        public async Task<List<AuditLog>> GetUserAuditLogsAsync(string userId, int page = 1, int pageSize = 50)
        {
            lock (_lock)
            {
                var logs = _auditLogs
                    .Where(log => log.UserId == userId)
                    .OrderByDescending(log => log.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return logs;
            }
        }

        public async Task<List<AuditLog>> GetEntityAuditLogsAsync(string entityType, string? entityId = null, int page = 1, int pageSize = 50)
        {
            lock (_lock)
            {
                var query = _auditLogs.Where(log => log.EntityType == entityType);
                
                if (!string.IsNullOrEmpty(entityId))
                {
                    query = query.Where(log => log.EntityId == entityId);
                }

                var logs = query
                    .OrderByDescending(log => log.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return logs;
            }
        }

        public async Task<List<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 50)
        {
            lock (_lock)
            {
                var logs = _auditLogs
                    .Where(log => log.Timestamp >= startDate && log.Timestamp <= endDate)
                    .OrderByDescending(log => log.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return logs;
            }
        }

        public async Task<List<AuditLog>> GetAuditLogsByActionAsync(string action, int page = 1, int pageSize = 50)
        {
            lock (_lock)
            {
                var logs = _auditLogs
                    .Where(log => log.Action == action)
                    .OrderByDescending(log => log.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return logs;
            }
        }

        public async Task<AuditLog?> GetAuditLogByIdAsync(int id)
        {
            lock (_lock)
            {
                var log = _auditLogs.FirstOrDefault(log => log.Id == id);
                return log;
            }
        }

        public async Task<bool> DeleteAuditLogAsync(int id)
        {
            lock (_lock)
            {
                var log = _auditLogs.FirstOrDefault(log => log.Id == id);
                if (log != null)
                {
                    _auditLogs.Remove(log);
                    return true;
                }
                return false;
            }
        }

        public async Task<int> GetAuditLogsCountAsync(string? userId = null, string? entityType = null, string? action = null)
        {
            lock (_lock)
            {
                var query = _auditLogs.AsQueryable();

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(log => log.UserId == userId);

                if (!string.IsNullOrEmpty(entityType))
                    query = query.Where(log => log.EntityType == entityType);

                if (!string.IsNullOrEmpty(action))
                    query = query.Where(log => log.Action == action);

                return query.Count();
            }
        }
    }
}
