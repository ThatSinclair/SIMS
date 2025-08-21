using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;
using System.Threading.Tasks;

namespace SIMSProject.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public User User { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string Notification { get; set; }

        public RegisterModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Password != ConfirmPassword)
            {
                Notification = "Passwords do not match.";
                return Page();
            }

            var existingUser = await _authService.GetUserByEmailAsync(User.Username);
            if (existingUser != null)
            {
                Notification = "Username already exists.";
                return Page();
            }

            User.PasswordHash = Password;
            await _authService.RegisterAsync(User);
            TempData["Notification"] = "Registration successful! Please log in.";
            return RedirectToPage("/Account/Login");
        }
    }
}