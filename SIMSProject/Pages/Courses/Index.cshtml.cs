using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;
using Microsoft.EntityFrameworkCore;

namespace SIMSProject.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public List<Course> Courses { get; set; }

        public async Task OnGetAsync()
        {
            Courses = await _courseService.GetAllCoursesAsync();

        }
    }
}