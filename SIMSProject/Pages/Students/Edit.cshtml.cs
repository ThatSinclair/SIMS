using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;

namespace SIMSProject.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly IStudentService _studentService;

        public EditModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _studentService.GetStudentByIdAsync(id); // Updated from GetStudentAsync
            if (Student == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _studentService.UpdateStudentAsync(Student);
            return RedirectToPage("/Index");
        }
    }
}