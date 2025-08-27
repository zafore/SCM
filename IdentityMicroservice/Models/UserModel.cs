namespace IdentityMicroservice.Models
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; } // Token expiration in minutes
    }
}
