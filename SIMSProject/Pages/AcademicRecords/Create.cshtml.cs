using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Models;
using SIMSProject.Services;

namespace SIMSProject.Pages.AcademicRecords
{
    public class CreateModel : PageModel
    {
        private readonly AcademicRecordService _service;

        [BindProperty]
        public AcademicRecord AcademicRecord { get; set; }

        public CreateModel(AcademicRecordService service)
        {
            _service = service;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _service.AddRecordAsync(AcademicRecord);
            return RedirectToPage("Index");
        }
    }
}
