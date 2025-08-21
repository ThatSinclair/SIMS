using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;

namespace SIMSProject.Pages.Tutors
{
    public class EditModel : PageModel
    {
        private readonly ITutorService _tutorService;

        public EditModel(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        [BindProperty]
        public Tutor Tutor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Tutor = await _tutorService.GetTutorAsync(id);
            if (Tutor == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _tutorService.UpdateTutorAsync(Tutor);
            return RedirectToPage("/Index");
        }
    }
}