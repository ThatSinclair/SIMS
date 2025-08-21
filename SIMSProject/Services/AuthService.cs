using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SIMSProject.Interfaces;
using SIMSProject.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SIMSProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _csvFilePath = "Data/users.csv";

        public async Task RegisterAsync(User user)
        {
            user.PasswordHash = HashPassword(user.PasswordHash);
            var users = await ReadUsersAsync();
            users.Add(user);
            await WriteUsersAsync(users);
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var users = await ReadUsersAsync();
            var user = users.FirstOrDefault(u => u.Username == username);
            return user != null && VerifyPassword(password, user.PasswordHash);
        }

        public async Task<bool> AuthorizeAsync(string username, string role)
        {
            var users = await ReadUsersAsync();
            var user = users.FirstOrDefault(u => u.Username == username);
            return user != null && user.Role == role;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            return await AuthenticateAsync(username, password);
        }

        public async Task<User?> GetUserByEmailAsync(string username)
        {
            var users = await ReadUsersAsync();
            return users.FirstOrDefault(u => u.Username == username);
        }

        public void Logout(HttpContext context)
        {
            context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
        }

        private string HashPassword(string password) => password; // Replace with real hashing
        private bool VerifyPassword(string password, string hash) => password == hash; // Replace with real verification

        private async Task<List<User>> ReadUsersAsync()
        {
            var users = new List<User>();
            if (!File.Exists(_csvFilePath))
                return users;

            var lines = await File.ReadAllLinesAsync(_csvFilePath);
            foreach (var line in lines.Skip(1)) // Skip header
            {
                var parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    users.Add(new User
                    {
                        Id = int.Parse(parts[0]),
                        Username = parts[1],
                        PasswordHash = parts[2],
                        Role = parts[3]
                    });
                }
            }
            return users;
        }

        private async Task WriteUsersAsync(List<User> users)
        {
            var lines = new List<string> { "Id,Username,PasswordHash,Role" };
            lines.AddRange(users.Select(u => $"{u.Id},{u.Username},{u.PasswordHash},{u.Role}"));
            await File.WriteAllLinesAsync(_csvFilePath, lines);
        }
    }
}