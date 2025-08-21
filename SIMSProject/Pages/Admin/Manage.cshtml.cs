using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SIMSProject.Interfaces;
using SIMSProject.Models;
using Microsoft.AspNetCore.Http;

namespace SIMSProject.Pages.Admin
{
    [Authorize]
    public class ManageModel : PageModel
    {
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly ICourseService _courseService;
        private readonly ICsvService _csvService;

        [TempData]
        public string StatusMessage { get; set; }

        public ManageModel(IStudentService studentService, ITutorService tutorService,
            ICourseService courseService, ICsvService csvService)
        {
            _studentService = studentService;
            _tutorService = tutorService;
            _courseService = courseService;
            _csvService = csvService;
        }

        public async Task<IActionResult> OnPostExportStudentsAsync()
        {
            var students = await _studentService.GetAllStudentsAsync();
            await _csvService.ExportToCsvAsync(students, "students.csv");
            StatusMessage = "Students exported successfully.";
            return Page();
        }

        public async Task<IActionResult> OnPostExportTutorsAsync()
        {
            var tutors = await _tutorService.GetAllTutorsAsync();
            await _csvService.ExportToCsvAsync(tutors, "tutors.csv");
            StatusMessage = "Tutors exported successfully.";
            return Page();
        }

        public async Task<IActionResult> OnPostExportCoursesAsync()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            await _csvService.ExportToCsvAsync(courses, "courses.csv");
            StatusMessage = "Courses exported successfully.";
            return Page();
        }

        public async Task<IActionResult> OnPostImportStudentsAsync(IFormFile studentsFile)
        {
            if (studentsFile != null && studentsFile.Length > 0)
            {
                var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "_students.csv");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await studentsFile.CopyToAsync(stream);
                }
                var students = await _csvService.ImportFromCsvAsync<Student>(filePath);
                foreach (var student in students)
                {
                    await _studentService.AddStudentAsync(student);
                }
                StatusMessage = "Students imported successfully.";
                return Page();
            }
            StatusMessage = "No file selected for import.";
            return Page();
        }

        public async Task<IActionResult> OnPostImportTutorsAsync(IFormFile tutorsFile)
        {
            if (tutorsFile != null && tutorsFile.Length > 0)
            {
                var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "_tutors.csv");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await tutorsFile.CopyToAsync(stream);
                }
                var tutors = await _csvService.ImportFromCsvAsync<Tutor>(filePath);
                foreach (var tutor in tutors)
                {
                    await _tutorService.AddTutorAsync(tutor);
                }
                StatusMessage = "Tutors imported successfully.";
                return Page();
            }
            StatusMessage = "No file selected for import.";
            return Page();
        }

        public async Task<IActionResult> OnPostImportCoursesAsync(IFormFile coursesFile)
        {
            if (coursesFile != null && coursesFile.Length > 0)
            {
                var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "_courses.csv");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await coursesFile.CopyToAsync(stream);
                }
                var courses = await _csvService.ImportFromCsvAsync<Course>(filePath);
                foreach (var course in courses)
                {
                    await _courseService.AddCourseAsync(course);
                }
                StatusMessage = "Courses imported successfully.";
                return Page();
            }
            StatusMessage = "No file selected for import.";
            return Page();
        }
    }
}