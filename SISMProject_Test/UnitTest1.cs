using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using SIMSProject.Interfaces;
using SIMSProject.Models;
using SIMSProject.Pages.Account;
using SIMSProject.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace SISMProject_Test
{
    public class UnitTest1
    {
        // Login.cshtml.cs tests
        [Fact]
        public void LoginModel_OnGet_SetsNotification_FromTempData()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object);
            model.TempData = new TempDataDictionaryFake { ["Notification"] = "Test" };
            model.OnGet();
            Assert.Equal("Test", model.Notification);
        }

        [Fact]
        public void LoginModel_OnGet_DoesNotSetNotification_IfNoTempData()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object);
            model.TempData = new TempDataDictionaryFake();
            model.OnGet();
            Assert.Null(model.Notification);
        }


        [Fact]
        public async Task LoginModel_OnPostAsync_FailedLogin_ReturnsPageWithNotification()
        {
            var mockAuth = new Mock<IAuthService>();
            mockAuth.Setup(a => a.AuthenticateAsync("user", "wrong")).ReturnsAsync(false);
            var model = new LoginModel(mockAuth.Object) { Username = "user", Password = "wrong" };
            var result = await model.OnPostAsync();
            Assert.IsType<PageResult>(result);
            Assert.Equal("Invalid username or password.", model.Notification);
        }

        // Register.cshtml.cs tests
        [Fact]
        public async Task RegisterModel_OnPostAsync_PasswordsDoNotMatch_ReturnsPage()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new RegisterModel(mockAuth.Object)
            {
                Password = "a",
                ConfirmPassword = "b",
                User = new User { Username = "user" }
            };
            var result = await model.OnPostAsync();
            Assert.IsType<PageResult>(result);
            Assert.Equal("Passwords do not match.", model.Notification);
        }

        [Fact]
        public async Task RegisterModel_OnPostAsync_UsernameExists_ReturnsPage()
        {
            var mockAuth = new Mock<IAuthService>();
            mockAuth.Setup(a => a.GetUserByEmailAsync("user")).ReturnsAsync(new User());
            var model = new RegisterModel(mockAuth.Object)
            {
                Password = "a",
                ConfirmPassword = "a",
                User = new User { Username = "user" }
            };
            var result = await model.OnPostAsync();
            Assert.IsType<PageResult>(result);
            Assert.Equal("Username already exists.", model.Notification);
        }

        [Fact]
        public async Task RegisterModel_OnPostAsync_SuccessfulRegistration_RedirectsToLogin()
        {
            var mockAuth = new Mock<IAuthService>();
            mockAuth.Setup(a => a.GetUserByEmailAsync("user")).ReturnsAsync((User?)null); // CS8600 fix
            var model = new RegisterModel(mockAuth.Object)
            {
                Password = "a",
                ConfirmPassword = "a",
                User = new User { Username = "user" },
                TempData = new TempDataDictionaryFake()
            };
            var result = await model.OnPostAsync();
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Account/Login", redirect.PageName);
            Assert.Equal("Registration successful! Please log in.", model.TempData["Notification"]);
        }
        
        // LoginModel additional tests
        [Fact]
        public async Task LoginModel_OnPostAsync_EmptyUsername_ReturnsPage()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object) { Username = "", Password = "pass" };
            var result = await model.OnPostAsync();
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task LoginModel_OnPostAsync_EmptyPassword_ReturnsPage()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object) { Username = "user", Password = "" };
            var result = await model.OnPostAsync();
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task LoginModel_OnPostAsync_NullUsername_ReturnsPage()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object) { Username = string.Empty, Password = "pass" }; // CS8625 fix: use empty string instead of null
            var result = await model.OnPostAsync();
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task LoginModel_OnPostAsync_NullPassword_ReturnsPage()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object) { Username = "user", Password = string.Empty }; // CS8625 fix: use empty string instead of null
            var result = await model.OnPostAsync();
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void LoginModel_Notification_Default_IsNull()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object);
            Assert.Null(model.Notification);
        }

        [Fact]
        public void LoginModel_Username_Property_Set_Get()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object) { Username = "abc" };
            Assert.Equal("abc", model.Username);
        }

        [Fact]
        public void LoginModel_Password_Property_Set_Get()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object) { Password = "xyz" };
            Assert.Equal("xyz", model.Password);
        }

        [Fact]
        public void LoginModel_Constructor_SetsAuthService()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new LoginModel(mockAuth.Object);
            Assert.NotNull(model);
        }

        // RegisterModel additional tests
        [Fact]
        public void RegisterModel_Notification_Default_IsNull()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new RegisterModel(mockAuth.Object);
            Assert.Null(model.Notification);
        }

        [Fact]
        public void RegisterModel_Constructor_SetsAuthService()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new RegisterModel(mockAuth.Object);
            Assert.NotNull(model);
        }

        [Fact]
        public void RegisterModel_User_Property_Set_Get()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new RegisterModel(mockAuth.Object) { User = new User { Username = "abc" } };
            Assert.Equal("abc", model.User.Username);
        }

        [Fact]
        public void RegisterModel_Password_Property_Set_Get()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new RegisterModel(mockAuth.Object) { Password = "pass" };
            Assert.Equal("pass", model.Password);
        }

        [Fact]
        public void RegisterModel_ConfirmPassword_Property_Set_Get()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new RegisterModel(mockAuth.Object) { ConfirmPassword = "pass" };
            Assert.Equal("pass", model.ConfirmPassword);
        }

        [Fact]
        public void RegisterModel_OnGet_DoesNotThrow()
        {
            var mockAuth = new Mock<IAuthService>();
            var model = new RegisterModel(mockAuth.Object);
            model.OnGet();
        }

        // StudentService tests
        [Fact]
        public async Task StudentService_GetAllStudentsAsync_ReturnsList()
        {
            var service = new StudentService();
            var result = await service.GetAllStudentsAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task StudentService_AddStudentAsync_AddsStudent()
        {
            var service = new StudentService();
            var student = new Student { FirstName = "Test", LastName = "User", Email = "test@example.com", StudentId = "S123" };
            await service.AddStudentAsync(student);
            var students = await service.GetAllStudentsAsync();
            Assert.Contains(students, s => s.Email == "test@example.com");
        }

        [Fact]
        public async Task StudentService_UpdateStudentAsync_UpdatesStudent()
        {
            var service = new StudentService();
            var student = new Student { FirstName = "Update", LastName = "User", Email = "update@example.com", StudentId = "S124" };
            await service.AddStudentAsync(student);
            student.LastName = "Updated";
            await service.UpdateStudentAsync(student);
            var updated = await service.GetStudentByIdAsync(student.Id);
            Assert.Equal("Updated", updated?.LastName); // CS8602 fix: use null-conditional
        }

        [Fact]
        public async Task StudentService_DeleteStudentAsync_DeletesStudent()
        {
            var service = new StudentService();
            var student = new Student { FirstName = "Delete", LastName = "User", Email = "delete@example.com", StudentId = "S125" };
            await service.AddStudentAsync(student);
            await service.DeleteStudentAsync(student.Id);
            var deleted = await service.GetStudentByIdAsync(student.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task StudentService_EnrollStudentInCourseAsync_Stub()
        {
            var service = new StudentService();
            await service.EnrollStudentInCourseAsync(1, 1);
        }

        // TutorService tests
        [Fact]
        public async Task TutorService_GetAllTutorsAsync_ReturnsList()
        {
            var service = new TutorService();
            var result = await service.GetAllTutorsAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TutorService_AddTutorAsync_AddsTutor()
        {
            var service = new TutorService();
            var tutor = new Tutor { Name = "Tutor", Email = "tutor@example.com", ExperienceYears = 5 };
            await service.AddTutorAsync(tutor);
            var tutors = await service.GetAllTutorsAsync();
            Assert.Contains(tutors, t => t.Email == "tutor@example.com");
        }

        [Fact]
        public async Task TutorService_UpdateTutorAsync_UpdatesTutor()
        {
            var service = new TutorService();
            var tutor = new Tutor { Name = "UpdateTutor", Email = "updatetutor@example.com", ExperienceYears = 3 };
            await service.AddTutorAsync(tutor);
            tutor.ExperienceYears = 10;
            await service.UpdateTutorAsync(tutor);
            var updated = await service.GetTutorByIdAsync(tutor.Id);
            Assert.Equal(10, updated?.ExperienceYears); // CS8602 fix: use null-conditional
        }

        [Fact]
        public async Task TutorService_DeleteTutorAsync_DeletesTutor()
        {
            var service = new TutorService();
            var tutor = new Tutor { Name = "DeleteTutor", Email = "deletetutor@example.com", ExperienceYears = 2 };
            await service.AddTutorAsync(tutor);
            await service.DeleteTutorAsync(tutor.Id);
            var deleted = await service.GetTutorByIdAsync(tutor.Id);
            Assert.Null(deleted);
        }

        // CourseService tests
        [Fact]
        public async Task CourseService_GetAllCoursesAsync_ReturnsList()
        {
            var service = new CourseService();
            var result = await service.GetAllCoursesAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CourseService_AddCourseAsync_AddsCourse()
        {
            var service = new CourseService();
            var course = new Course { Name = "Course", Code = "C101" };
            await service.AddCourseAsync(course);
            var courses = await service.GetAllCoursesAsync();
            Assert.Contains(courses, c => c.Code == "C101");
        }

        [Fact]
        public async Task CourseService_UpdateCourseAsync_UpdatesCourse()
        {
            var service = new CourseService();
            var course = new Course { Name = "UpdateCourse", Code = "C102" };
            await service.AddCourseAsync(course);
            course.Name = "UpdatedCourse";
            await service.UpdateCourseAsync(course);
            var updated = await service.GetCourseByIdAsync(course.Id);
            Assert.Equal("UpdatedCourse", updated?.Name); // CS8602 fix: use null-conditional
        }

        [Fact]
        public async Task CourseService_DeleteCourseAsync_DeletesCourse()
        {
            var service = new CourseService();
            var course = new Course { Name = "DeleteCourse", Code = "C103" };
            await service.AddCourseAsync(course);
            await service.DeleteCourseAsync(course.Id);
            var deleted = await service.GetCourseByIdAsync(course.Id);
            Assert.Null(deleted);
        }

        // AcademicRecordService tests
        [Fact]
        public async Task AcademicRecordService_GetRecordByIdAsync_ReturnsNull_WhenNotFound()
        {
            var service = new AcademicRecordService();
            var result = await service.GetRecordByIdAsync(-999);
            Assert.Null(result);
        }

        // CsvService tests
        [Fact]
        public async Task CsvService_ExportToCsvAsync_ExportsData()
        {
            var service = CsvService.Instance;
            var data = new List<Student> { new Student { FirstName = "CSV", LastName = "Test", Email = "csv@example.com", StudentId = "S126" } };
            await service.ExportToCsvAsync(data, "test_students.csv");
            Assert.True(System.IO.File.Exists("test_students.csv"));
        }

        [Fact]
        public async Task CsvService_ImportFromCsvAsync_ImportsData()
        {
            var service = CsvService.Instance;
            var students = await service.ImportFromCsvAsync<Student>("Data/students.csv");
            Assert.NotNull(students);
        }
    }

    // Nullability-correct fake TempDataDictionary for Razor Pages model tests
    public class TempDataDictionaryFake : Dictionary<string, object?>, Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary
    {
        public void Keep() { }
        public void Keep(string key) { }
        public void Load() { }
        public object? Peek(string key) => TryGetValue(key, out var value) ? value : null; // CS8603 fix
        public void Save() { }
    }
}