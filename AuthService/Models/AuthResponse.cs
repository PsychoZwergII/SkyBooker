namespace AuthService.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
} 