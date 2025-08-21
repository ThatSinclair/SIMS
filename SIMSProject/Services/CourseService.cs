using SIMSProject.Models;
using SIMSProject.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SIMSProject.Services
{
    public class CourseService : ICourseService
    {
        private readonly string _csvFilePath = "Data/courses.csv";
        private readonly ITutorService _tutorService;

        public CourseService(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        public CourseService()
        {
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await ReadCoursesAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            var courses = await ReadCoursesAsync();
            return courses.FirstOrDefault(c => c.Id == id);
        }

        public async Task AddCourseAsync(Course course)
        {
            var courses = await ReadCoursesAsync();
            course.Id = courses.Any() ? courses.Max(c => c.Id) + 1 : 1;
            courses.Add(course);
            await WriteCoursesAsync(courses);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var courses = await ReadCoursesAsync();
            var index = courses.FindIndex(c => c.Id == course.Id);
            if (index >= 0)
            {
                courses[index] = course;
                await WriteCoursesAsync(courses);
            }
        }

        public async Task DeleteCourseAsync(int id)
        {
            var courses = await ReadCoursesAsync();
            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                courses.Remove(course);
                await WriteCoursesAsync(courses);
            }
        }

        public async Task<Course?> GetCourseAsync(int id)
        {
            return await GetCourseByIdAsync(id);
        }

        private async Task<List<Course>> ReadCoursesAsync()
        {
            var courses = new List<Course>();
            if (!File.Exists(_csvFilePath))
                return courses;

            var lines = await File.ReadAllLinesAsync(_csvFilePath);
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length >= 6) // Id, Title, Description, AssignedTutorId, Code, Name
                {
                    var course = new Course
                    {
                        Id = int.Parse(parts[0]),
                        Title = parts[1],
                        Description = parts[2],
                        AssignedTutorId = string.IsNullOrEmpty(parts[3]) ? null : int.Parse(parts[3]),
                        Code = parts[4],
                        Name = parts[5]
                    };
                    if (course.AssignedTutorId.HasValue && _tutorService != null)
                    {
                        var tutor = await _tutorService.GetTutorByIdAsync(course.AssignedTutorId.Value);
                        course.AssignedTutor = tutor ?? new Tutor { Name = "N/A", Email = null, ExperienceYears = 0 };
                    }
                    courses.Add(course);
                }
            }
            return courses;
        }

        private async Task WriteCoursesAsync(List<Course> courses)
        {
            var directory = Path.GetDirectoryName(_csvFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Add Code and Name to the header and output
            var lines = new List<string> { "Id,Title,Description,AssignedTutorId,Code,Name" };
            lines.AddRange(courses.Select(c =>
                $"{c.Id},{c.Title},{c.Description},{(c.AssignedTutorId.HasValue ? c.AssignedTutorId.Value.ToString() : "")},{c.Code},{c.Name}"));
            await File.WriteAllLinesAsync(_csvFilePath, lines);
        }
    }
}