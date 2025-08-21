namespace SIMSProject.Interfaces
{
    public interface ICsvService
    {
        Task ExportToCsvAsync<T>(List<T> data, string filePath);
        Task<List<T>> ImportFromCsvAsync<T>(string filePath);
    }
}