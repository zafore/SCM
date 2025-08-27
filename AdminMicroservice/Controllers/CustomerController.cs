using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminMicroservice.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class CustomerController : ControllerBase
    {
        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetCustomer()
        {
            return Ok("ok Customner");
        }

    }

    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Policy = "AdminPolicy")]
    public class UserManagementController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            // TODO: Implement get all users
            return Ok("Get all users");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            // TODO: Implement create user
            return Ok("User created");
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest request)
        {
            // TODO: Implement update user
            return Ok($"User {userId} updated");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            // TODO: Implement delete user
            return Ok($"User {userId} deleted");
        }

        [HttpPost("{userId}/roles")]
        public async Task<IActionResult> AssignRole(int userId, [FromBody] AssignRoleRequest request)
        {
            // TODO: Implement assign role
            return Ok($"Role assigned to user {userId}");
        }

        [HttpDelete("{userId}/roles/{roleId}")]
        public async Task<IActionResult> RemoveRole(int userId, int roleId)
        {
            // TODO: Implement remove role
            return Ok($"Role {roleId} removed from user {userId}");
        }
    }

    [ApiController]
    [Route("api/admin/roles")]
    [Authorize(Policy = "AdminPolicy")]
    public class RoleManagementController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            // TODO: Implement get all roles
            return Ok("Get all roles");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            // TODO: Implement create role
            return Ok("Role created");
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] UpdateRoleRequest request)
        {
            // TODO: Implement update role
            return Ok($"Role {roleId} updated");
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            // TODO: Implement delete role
            return Ok($"Role {roleId} deleted");
        }
    }

    [ApiController]
    [Route("api/admin/permissions")]
    [Authorize(Policy = "AdminPolicy")]
    public class PermissionManagementController : ControllerBase
    {
        [HttpGet("routes")]
        public async Task<IActionResult> GetRoutePermissions()
        {
            // TODO: Implement get route permissions
            return Ok("Get route permissions");
        }

        [HttpPost("routes/{routeId}/roles")]
        public async Task<IActionResult> AssignRouteToRole(string routeId, [FromBody] AssignRouteToRoleRequest request)
        {
            // TODO: Implement assign route to role
            return Ok($"Route {routeId} assigned to role");
        }

        [HttpDelete("routes/{routeId}/roles/{roleId}")]
        public async Task<IActionResult> RemoveRouteFromRole(string routeId, int roleId)
        {
            // TODO: Implement remove route from role
            return Ok($"Route {routeId} removed from role {roleId}");
        }
    }

    // DTOs
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<int> RoleIds { get; set; } = new();
    }

    public class UpdateUserRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<int> RoleIds { get; set; } = new();
    }

    public class AssignRoleRequest
    {
        public int RoleId { get; set; }
    }

    public class CreateRoleRequest
    {
        public string RoleName { get; set; }
        public int RoleTypeId { get; set; }
    }

    public class UpdateRoleRequest
    {
        public string RoleName { get; set; }
        public int RoleTypeId { get; set; }
    }

    public class AssignRouteToRoleRequest
    {
        public int RoleId { get; set; }
        public List<string> AllowedMethods { get; set; } = new();
    }
}
