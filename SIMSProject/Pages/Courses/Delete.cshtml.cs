using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;

namespace SIMSProject.Pages.Courses
{
    public class DeleteModel : PageModel
    {
        private readonly ICourseService _courseService;

        public DeleteModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _courseService.GetCourseAsync(id);
            if (Course == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _courseService.DeleteCourseAsync(id);
            return RedirectToPage("/Index");
        }
    }
}