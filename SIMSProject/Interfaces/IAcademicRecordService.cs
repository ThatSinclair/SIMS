using SIMSProject.Models;

namespace SIMSProject.Interfaces
{
    public interface IAcademicRecordService
    {
        Task<List<AcademicRecord>> GetAllRecordsAsync();
        Task<AcademicRecord> GetRecordByIdAsync(int id);
        Task AddRecordAsync(AcademicRecord record);
        Task UpdateRecordAsync(AcademicRecord record);
        Task DeleteRecordAsync(int id);
    }
}