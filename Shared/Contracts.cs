namespace Shared
{
    public class Contracts
    {
        public class UserContract
        {
            public string UserId { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }

        // DTO for response after login (might be used by Gateway or UI)
        public class AuthResponseContract
        {
            public string Token { get; set; }
            public int ExpiresIn { get; set; } // Token expiration in minutes
        }
    }
}
