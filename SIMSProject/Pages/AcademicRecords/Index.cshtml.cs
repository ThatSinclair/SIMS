using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;

namespace SIMSProject.Pages.AcademicRecords
{
    public class IndexModel : PageModel
    {
        private readonly IAcademicRecordService _recordService;

        public IndexModel(IAcademicRecordService recordService)
        {
            _recordService = recordService;
        }

        public List<AcademicRecord> Records { get; set; }

        public async Task OnGetAsync()
        {
            Records = await _recordService.GetAllRecordsAsync();
        }
    }
}