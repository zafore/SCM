using System.Security.Claims;
using System.Text.Json;

namespace APIGateWay.Services
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(string route, string method, ClaimsPrincipal user);
        Task<List<string>> GetUserRolesAsync(ClaimsPrincipal user);
        Task<bool> IsRoleAllowedAsync(string userRole, string requiredRole);
    }

    public class PermissionService : IPermissionService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PermissionService> _logger;
        private readonly Dictionary<string, Dictionary<string, List<string>>> _routePermissions;
        private readonly Dictionary<string, List<string>> _roleHierarchy;

        public PermissionService(IConfiguration configuration, ILogger<PermissionService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            
            // Load permissions from configuration
            var permissionsConfig = _configuration.GetSection("RoutePermissions");
            _routePermissions = permissionsConfig.Get<Dictionary<string, Dictionary<string, List<string>>>>() ?? new();
            
            var hierarchyConfig = _configuration.GetSection("RoleHierarchy");
            _roleHierarchy = hierarchyConfig.Get<Dictionary<string, List<string>>>() ?? new();
        }

        public async Task<bool> HasPermissionAsync(string route, string method, ClaimsPrincipal user)
        {
            try
            {
                var userRoles = await GetUserRolesAsync(user);
                
                // Check if route exists in permissions
                if (!_routePermissions.ContainsKey(route))
                {
                    _logger.LogWarning($"Route {route} not found in permissions configuration");
                    return false;
                }

                var routePermissions = _routePermissions[route];
                
                // Check if method exists for this route
                if (!routePermissions.ContainsKey(method))
                {
                    _logger.LogWarning($"Method {method} not found for route {route}");
                    return false;
                }

                var allowedRoles = routePermissions[method];
                
                // Check if user has any of the allowed roles
                foreach (var userRole in userRoles)
                {
                    if (allowedRoles.Contains(userRole))
                    {
                        return true;
                    }
                    
                    // Check role hierarchy
                    if (_roleHierarchy.ContainsKey(userRole))
                    {
                        var inheritedRoles = _roleHierarchy[userRole];
                        if (inheritedRoles.Any(role => allowedRoles.Contains(role)))
                        {
                            return true;
                        }
                    }
                }

                _logger.LogWarning($"User with roles {string.Join(", ", userRoles)} denied access to {method} {route}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking permissions for {method} {route}");
                return false;
            }
        }

        public async Task<List<string>> GetUserRolesAsync(ClaimsPrincipal user)
        {
            var roles = new List<string>();
            
            if (user?.Identity?.IsAuthenticated == true)
            {
                var roleClaims = user.FindAll(ClaimTypes.Role);
                roles.AddRange(roleClaims.Select(c => c.Value));
            }

            return await Task.FromResult(roles);
        }

        public async Task<bool> IsRoleAllowedAsync(string userRole, string requiredRole)
        {
            if (userRole == requiredRole)
                return true;

            if (_roleHierarchy.ContainsKey(userRole))
            {
                return _roleHierarchy[userRole].Contains(requiredRole);
            }

            return false;
        }
    }
}
