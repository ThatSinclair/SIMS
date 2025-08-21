using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;

namespace SIMSProject.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly IStudentService _studentService;

        public CreateModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [BindProperty]
        public Student Student { get; set; }

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

            await _studentService.AddStudentAsync(Student);
            return RedirectToPage("Index");
        }
    }
}