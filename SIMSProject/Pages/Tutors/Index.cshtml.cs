using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;

namespace SIMSProject.Pages.Tutors
{
    public class IndexModel : PageModel
    {
        private readonly ITutorService _tutorService;

        public IndexModel(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        public List<Tutor> Tutors { get; set; }

        public async Task OnGetAsync()
        {
            Tutors = await _tutorService.GetAllTutorsAsync();
        }
    }
}