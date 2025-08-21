using System.IO;
using CsvHelper;
using SIMSProject.Interfaces;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMSProject.Services
{
    public class CsvService : ICsvService
    {
        private static readonly CsvService _instance = new CsvService(); // Singleton Pattern

        private CsvService() { } // Private constructor for Singleton

        public static CsvService Instance => _instance;

        public async Task ExportToCsvAsync<T>(List<T> data, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(data);
            }
        }

        public async Task<List<T>> ImportFromCsvAsync<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = new List<T>();
                await foreach (var record in csv.GetRecordsAsync<T>())
                {
                    records.Add(record);
                }
                return records;
            }
        }
    }
}