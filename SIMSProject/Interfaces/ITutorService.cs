using SIMSProject.Models;

namespace SIMSProject.Interfaces
{
    public interface ITutorService
    {
        Task<List<Tutor>> GetAllTutorsAsync();
        Task<Tutor> GetTutorByIdAsync(int id);
        Task AddTutorAsync(Tutor tutor);
        Task UpdateTutorAsync(Tutor tutor);
        Task DeleteTutorAsync(int id);
        Task<Tutor> GetTutorAsync(int id);
    }
}