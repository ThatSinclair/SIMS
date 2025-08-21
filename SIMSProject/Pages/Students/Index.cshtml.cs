using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;

namespace SIMSProject.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly IStudentService _studentService;

        public IndexModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public List<Student> Students { get; set; }

        public async Task OnGetAsync()
        {
            Students = await _studentService.GetAllStudentsAsync();
        }
    }
}