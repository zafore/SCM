using APIGateWay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGateWay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<AuditController> _logger;

        public AuditController(IAuditService auditService, ILogger<AuditController> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        /// <summary>
        /// Get audit logs for a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,SuperAdmin,Manager")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserAuditLogs(
            string userId, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var logs = await _auditService.GetUserAuditLogsAsync(userId, page, pageSize);
                var totalCount = await _auditService.GetAuditLogsCountAsync(userId: userId);

                return Ok(new
                {
                    Data = logs,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user audit logs for {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get audit logs for a specific entity
        /// </summary>
        [HttpGet("entity/{entityType}")]
        [Authorize(Roles = "Admin,SuperAdmin,Manager")]
        public async Task<ActionResult<IEnumerable<object>>> GetEntityAuditLogs(
            string entityType,
            [FromQuery] string? entityId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var logs = await _auditService.GetEntityAuditLogsAsync(entityType, entityId, page, pageSize);
                var totalCount = await _auditService.GetAuditLogsCountAsync(entityType: entityType);

                return Ok(new
                {
                    Data = logs,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity audit logs for {EntityType}", entityType);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get audit logs by date range
        /// </summary>
        [HttpGet("date-range")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<IEnumerable<object>>> GetAuditLogsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var logs = await _auditService.GetAuditLogsByDateRangeAsync(startDate, endDate, page, pageSize);
                var totalCount = await _auditService.GetAuditLogsCountAsync();

                return Ok(new
                {
                    Data = logs,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    StartDate = startDate,
                    EndDate = endDate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit logs by date range");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get audit logs by action type
        /// </summary>
        [HttpGet("action/{action}")]
        [Authorize(Roles = "Admin,SuperAdmin,Manager")]
        public async Task<ActionResult<IEnumerable<object>>> GetAuditLogsByAction(
            string action,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var logs = await _auditService.GetAuditLogsByActionAsync(action, page, pageSize);
                var totalCount = await _auditService.GetAuditLogsCountAsync(action: action);

                return Ok(new
                {
                    Data = logs,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    Action = action
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit logs by action {Action}", action);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get specific audit log by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin,Manager")]
        public async Task<ActionResult<object>> GetAuditLog(int id)
        {
            try
            {
                var log = await _auditService.GetAuditLogByIdAsync(id);
                if (log == null)
                {
                    return NotFound($"Audit log with ID {id} not found");
                }

                return Ok(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit log {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get audit statistics
        /// </summary>
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<object>> GetAuditStatistics()
        {
            try
            {
                var totalLogs = await _auditService.GetAuditLogsCountAsync();
                var loginCount = await _auditService.GetAuditLogsCountAsync(action: "LOGIN");
                var createCount = await _auditService.GetAuditLogsCountAsync(action: "CREATE");
                var updateCount = await _auditService.GetAuditLogsCountAsync(action: "UPDATE");
                var deleteCount = await _auditService.GetAuditLogsCountAsync(action: "DELETE");

                return Ok(new
                {
                    TotalLogs = totalLogs,
                    LoginCount = loginCount,
                    CreateCount = createCount,
                    UpdateCount = updateCount,
                    DeleteCount = deleteCount,
                    GeneratedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit statistics");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get recent audit logs (last 24 hours)
        /// </summary>
        [HttpGet("recent")]
        [Authorize(Roles = "Admin,SuperAdmin,Manager")]
        public async Task<ActionResult<IEnumerable<object>>> GetRecentAuditLogs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var endDate = DateTime.UtcNow;
                var startDate = endDate.AddHours(-24);

                var logs = await _auditService.GetAuditLogsByDateRangeAsync(startDate, endDate, page, pageSize);

                return Ok(new
                {
                    Data = logs,
                    Page = page,
                    PageSize = pageSize,
                    TimeRange = "Last 24 hours"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recent audit logs");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete audit log (SuperAdmin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteAuditLog(int id)
        {
            try
            {
                var result = await _auditService.DeleteAuditLogAsync(id);
                if (!result)
                {
                    return NotFound($"Audit log with ID {id} not found");
                }

                _logger.LogInformation("Audit log {Id} deleted by {UserId}", id, User.Identity?.Name);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting audit log {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get my audit logs (current user)
        /// </summary>
        [HttpGet("my-logs")]
        public async Task<ActionResult<IEnumerable<object>>> GetMyAuditLogs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found");
                }

                var logs = await _auditService.GetUserAuditLogsAsync(userId, page, pageSize);
                var totalCount = await _auditService.GetAuditLogsCountAsync(userId: userId);

                return Ok(new
                {
                    Data = logs,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current user audit logs");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
