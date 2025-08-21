using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIMSProject.Pages.Courses
{
    public class EditModel : PageModel
    {
        private readonly ICourseService _courseService;
        private readonly ITutorService _tutorService;

        public EditModel(ICourseService courseService, ITutorService tutorService)
        {
            _courseService = courseService;
            _tutorService = tutorService;
        }

        [BindProperty]
        public Course Course { get; set; }

        public SelectList TutorList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _courseService.GetCourseByIdAsync(id);
            if (Course == null) return NotFound();

            var tutors = await _tutorService.GetAllTutorsAsync();
            TutorList = new SelectList(tutors, "Id", "Name", Course.AssignedTutorId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _courseService.UpdateCourseAsync(Course);
            return RedirectToPage("/Index");
        }
    }
}