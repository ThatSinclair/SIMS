using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;

namespace SIMSProject.Pages.Tutors
{
    public class CreateModel : PageModel
    {
        private readonly ITutorService _tutorService;

        public CreateModel(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        [BindProperty]
        public Tutor Tutor { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _tutorService.AddTutorAsync(Tutor);
            return RedirectToPage("/Index");
        }
    }
}