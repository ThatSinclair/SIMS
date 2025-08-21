using SIMSProject.Models;
using System.Threading.Tasks;

namespace SIMSProject.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(User user);
        Task<bool> AuthenticateAsync(string username, string password);
        Task<bool> AuthorizeAsync(string username, string role);
        Task<bool> LoginAsync(string username, string password);
        Task<User?> GetUserByEmailAsync(string username);

        void Logout(HttpContext context);
    }
}
