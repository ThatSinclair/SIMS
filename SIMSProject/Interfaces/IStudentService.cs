using SIMSProject.Models;
using System.Threading.Tasks;

namespace SIMSProject.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id); // Updated
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
        Task EnrollStudentInCourseAsync(int studentId, int courseId);
    }
}