using SIMSProject.Interfaces;
using SIMSProject.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SIMSProject.Services
{
    public class AcademicRecordService : IAcademicRecordService
    {
        private readonly string _csvFilePath = "Data/academic_records.csv";

        public async Task<List<AcademicRecord>> GetAllRecordsAsync()
        {
            return await ReadRecordsAsync();
        }

        public async Task<AcademicRecord> GetRecordByIdAsync(int id)
        {
            var records = await ReadRecordsAsync();
            return records.FirstOrDefault(r => r.Id == id);
        }

        public async Task AddRecordAsync(AcademicRecord record)
        {
            var records = await ReadRecordsAsync();
            record.Id = records.Any() ? records.Max(r => r.Id) + 1 : 1;
            records.Add(record);
            await WriteRecordsAsync(records);
        }

        public async Task UpdateRecordAsync(AcademicRecord record)
        {
            var records = await ReadRecordsAsync();
            var index = records.FindIndex(r => r.Id == record.Id);
            if (index >= 0)
            {
                records[index] = record;
                await WriteRecordsAsync(records);
            }
        }

        public async Task DeleteRecordAsync(int id)
        {
            var records = await ReadRecordsAsync();
            var record = records.FirstOrDefault(r => r.Id == id);
            if (record != null)
            {
                records.Remove(record);
                await WriteRecordsAsync(records);
            }
        }

        private async Task<List<AcademicRecord>> ReadRecordsAsync()
        {
            var records = new List<AcademicRecord>();
            if (!File.Exists(_csvFilePath))
                return records;

            var lines = await File.ReadAllLinesAsync(_csvFilePath);
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    records.Add(new AcademicRecord
                    {
                        Id = int.Parse(parts[0]),
                        StudentId = int.Parse(parts[1]),
                        CourseId = int.Parse(parts[2]),
                        Grade = parts[3]
                    });
                }
            }
            return records;
        }

        private async Task WriteRecordsAsync(List<AcademicRecord> records)
        {
            var lines = new List<string> { "Id,StudentId,CourseId,Grade" };
            lines.AddRange(records.Select(r => $"{r.Id},{r.StudentId},{r.CourseId},{r.Grade}"));
            await File.WriteAllLinesAsync(_csvFilePath, lines);
        }
    }
}