using APIGateWay.Models;

namespace APIGateWay.Services
{
    public interface IAuditService
    {
        Task LogAsync(AuditLogRequest auditRequest);
        Task LogLoginAsync(string userId, string userName, string userEmail, string ipAddress, string userAgent, string? role = null);
        Task LogLogoutAsync(string userId, string userName, string ipAddress);
        Task LogActionAsync(string userId, string userName, string action, string entityType, string? entityId = null, string? entityName = null, string? changes = null);
        Task LogErrorAsync(string userId, string userName, string action, string entityType, string errorMessage, string? entityId = null);
        Task<List<AuditLog>> GetUserAuditLogsAsync(string userId, int page = 1, int pageSize = 50);
        Task<List<AuditLog>> GetEntityAuditLogsAsync(string entityType, string? entityId = null, int page = 1, int pageSize = 50);
        Task<List<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 50);
        Task<List<AuditLog>> GetAuditLogsByActionAsync(string action, int page = 1, int pageSize = 50);
        Task<AuditLog?> GetAuditLogByIdAsync(int id);
        Task<bool> DeleteAuditLogAsync(int id);
        Task<int> GetAuditLogsCountAsync(string? userId = null, string? entityType = null, string? action = null);
    }
}
