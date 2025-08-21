using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SIMSProject.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Notification { get; set; }

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
            if (TempData.ContainsKey("Notification"))
            {
                Notification = TempData["Notification"] as string;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _authService.AuthenticateAsync(Username, Password))
            {
                // Get the user and their role
                var user = await _authService.GetUserByEmailAsync(Username);

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                TempData["Notification"] = "Login successful!";
                return RedirectToPage("/Index"); // Always redirect to home
            }
            Notification = "Invalid username or password.";
            return Page();
        }
    }
}