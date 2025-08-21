using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;

namespace SIMSProject.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IAuthService _authService;

        public LogoutModel(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult OnPost()
        {
            _authService.Logout(HttpContext);
            return RedirectToPage("/Index");
        }
    }
}