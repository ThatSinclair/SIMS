using SIMSProject.Interfaces;
using SIMSProject.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SIMSProject.Services
{
    public class StudentService : IStudentService
    {
        private readonly string _csvFilePath = "Data/students.csv";

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await ReadStudentsAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var students = await ReadStudentsAsync();
            return students.FirstOrDefault(s => s.Id == id);
        }

        public async Task AddStudentAsync(Student student)
        {
            var students = await ReadStudentsAsync();
            student.Id = students.Any() ? students.Max(s => s.Id) + 1 : 1;
            students.Add(student);
            await WriteStudentsAsync(students);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            var students = await ReadStudentsAsync();
            var index = students.FindIndex(s => s.Id == student.Id);
            if (index >= 0)
            {
                students[index] = student;
                await WriteStudentsAsync(students);
            }
        }

        public async Task DeleteStudentAsync(int id)
        {
            var students = await ReadStudentsAsync();
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                students.Remove(student);
                await WriteStudentsAsync(students);
            }
        }

        // Implementation added for interface method
        public Task EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            // This is a stub implementation. You should replace it with actual logic.
            return Task.CompletedTask;
        }

        private async Task<List<Student>> ReadStudentsAsync()
        {
            var students = new List<Student>();
            if (!File.Exists(_csvFilePath))
                return students;

            var lines = await File.ReadAllLinesAsync(_csvFilePath);
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    students.Add(new Student
                    {
                        Id = int.Parse(parts[0]),
                        FirstName = parts[1],
                        LastName = parts[2],
                        Email = parts[3],
                        StudentId = parts[4]
                    });
                }
            }
            return students;
        }

        private async Task WriteStudentsAsync(List<Student> students)
        {
            var lines = new List<string> { "Id,FirstName,LastName,Email,StudentId" };
            lines.AddRange(students.Select(s => $"{s.Id},{s.FirstName},{s.LastName},{s.Email},{s.StudentId}"));
            await File.WriteAllLinesAsync(_csvFilePath, lines);
        }
    }
}