using Microsoft.EntityFrameworkCore;
using SIMSProject.Data;
using SIMSProject.Interfaces;
using SIMSProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Account/Register");
});
builder.Services.AddDbContext<SIMSContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<IAcademicRecordService, AcademicRecordService>();
builder.Services.AddScoped<AcademicRecordService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddSingleton<ICsvService>(CsvService.Instance);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login"; // Set your login page
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
