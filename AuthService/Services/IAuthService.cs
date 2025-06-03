using AuthService.Models;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<User> Register(User user);
        Task<string> Login(LoginModel model);
    }
} 