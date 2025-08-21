using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Models;
using SIMSProject.Services;

namespace SIMSProject.Pages.AcademicRecords
{
    public class DeleteModel : PageModel
    {
        private readonly AcademicRecordService _service;

        [BindProperty]
        public AcademicRecord AcademicRecord { get; set; }

        public DeleteModel(AcademicRecordService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            AcademicRecord = await _service.GetRecordByIdAsync(id);
            if (AcademicRecord == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _service.DeleteRecordAsync(id);
            return RedirectToPage("Index");
        }
    }
}
