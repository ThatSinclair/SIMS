using Moq;
using SIMSProject.Data;
using SIMSProject.Models;
using SIMSProject.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SIMSProject.UnitTests
{
    public class SimpleUnitTests
    {
        [Fact]
        public void Tutor_Model_Properties_Work()
        {
            var tutor = new Tutor { Id = 1, Name = "Dr. X", Email = "x@y.com", ExperienceYears = 5 };
            Assert.Equal(1, tutor.Id);
            Assert.Equal("Dr. X", tutor.Name);
            Assert.Equal("x@y.com", tutor.Email);
            Assert.Equal(5, tutor.ExperienceYears);
        }

        [Fact]
        public void AcademicRecord_Model_Properties_Work()
        {
            var record = new AcademicRecord { Id = 1, StudentId = 2, CourseId = 3, Grade = "A" };
            Assert.Equal(1, record.Id);
            Assert.Equal(2, record.StudentId);
            Assert.Equal(3, record.CourseId);
            Assert.Equal("A", record.Grade);
        }

        [Fact]
        public void User_Model_Properties_Work()
        {
            var user = new User { Id = 1, Username = "user", PasswordHash = "hash", Role = "Admin" };
            Assert.Equal(1, user.Id);
            Assert.Equal("user", user.Username);
            Assert.Equal("hash", user.PasswordHash);
            Assert.Equal("Admin", user.Role);
        }

        [Fact]
        public async Task StudentService_Add_And_Get_Student()
        {
            var service = new StudentService();
            var student = new Student { FirstName = "Test", LastName = "User", Email = "test@user.com", StudentId = "S100" };
            await service.AddStudentAsync(student);
            var students = await service.GetAllStudentsAsync();
            Assert.Contains(students, s => s.StudentId == "S100");
        }

        [Fact]
        public async Task StudentService_Update_Student()
        {
            var service = new StudentService();
            var student = new Student { FirstName = "Jane", LastName = "Doe", Email = "jane@doe.com", StudentId = "S101" };
            await service.AddStudentAsync(student);
            var students = await service.GetAllStudentsAsync();
            var toUpdate = students.Find(s => s.StudentId == "S101");
            toUpdate.LastName = "Smith";
            await service.UpdateStudentAsync(toUpdate);
            var updated = await service.GetStudentByIdAsync(toUpdate.Id);
            Assert.Equal("Smith", updated.LastName);
        }

        [Fact]
        public async Task StudentService_Delete_Student()
        {
            var service = new StudentService();
            var student = new Student { FirstName = "Delete", LastName = "Me", Email = "delete@me.com", StudentId = "S102" };
            await service.AddStudentAsync(student);
            var students = await service.GetAllStudentsAsync();
            var toDelete = students.Find(s => s.StudentId == "S102");
            await service.DeleteStudentAsync(toDelete.Id);
            var deleted = await service.GetStudentByIdAsync(toDelete.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task CourseService_Add_And_Get_Course()
        {
            var service = new CourseService();
            var course = new Course { Title = "Physics", Description = "Intro", AssignedTutorId = null };
            await service.AddCourseAsync(course);
            var courses = await service.GetAllCoursesAsync();
            Assert.Contains(courses, c => c.Title == "Physics");
        }

        [Fact]
        public async Task TutorService_Add_And_Get_Tutor()
        {
            var service = new TutorService();
            var tutor = new Tutor { Name = "Tutor1", Email = "tutor1@edu.com", ExperienceYears = 2 };
            await service.AddTutorAsync(tutor);
            var tutors = await service.GetAllTutorsAsync();
            Assert.Contains(tutors, t => t.Name == "Tutor1");
        }

        [Fact]
        public async Task AcademicRecordService_Add_And_Get_Record()
        {
            var service = new AcademicRecordService();
            var record = new AcademicRecord { StudentId = 1, CourseId = 1, Grade = "B" };
            await service.AddRecordAsync(record);
            var records = await service.GetAllRecordsAsync();
            Assert.Contains(records, r => r.Grade == "B");
        }

        [Fact]
        public async Task AuthService_Register_And_Authenticate_User()
        {
            var service = new AuthService();
            var user = new User { Username = "testuser", PasswordHash = "pass", Role = "Student" };
            await service.RegisterAsync(user);
            var result = await service.AuthenticateAsync("testuser", "pass");
            Assert.True(result);
        }

        [Fact]
        public async Task AuthService_Authorize_User()
        {
            var service = new AuthService();
            var user = new User { Username = "admin", PasswordHash = "adminpass", Role = "Admin" };
            await service.RegisterAsync(user);
            var result = await service.AuthorizeAsync("admin", "Admin");
            Assert.True(result);
        }

        [Fact]
        public async Task AuthService_Login_User()
        {
            var service = new AuthService();
            var user = new User { Username = "loginuser", PasswordHash = "loginpass", Role = "Student" };
            await service.RegisterAsync(user);
            var result = await service.LoginAsync("loginuser", "loginpass");
            Assert.True(result);
        }

        [Fact]
        public async Task AuthService_GetUserByEmail_Returns_User()
        {
            var service = new AuthService();
            var user = new User { Username = "findme", PasswordHash = "findpass", Role = "Student" };
            await service.RegisterAsync(user);
            var found = await service.GetUserByEmailAsync("findme");
            Assert.NotNull(found);
            Assert.Equal("findme", found.Username);
        }

        [Fact]
        public void Student_Default_Collections_Are_NotNull()
        {
            var student = new Student();
            Assert.NotNull(student.Courses);
            Assert.NotNull(student.AcademicRecords);
        }

        [Fact]
        public void Tutor_Default_Courses_Are_NotNull()
        {
            var tutor = new Tutor();
            Assert.NotNull(tutor.Courses);
        }

        [Fact]
        public void AcademicRecord_Model_Student_And_Course_Are_Null_ByDefault()
        {
            var record = new AcademicRecord();
            Assert.Null(record.Student);
            Assert.Null(record.Course);
        }
    }
}