using SIMSProject.Models;
using SIMSProject.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SIMSProject.Services
{
    public class TutorService : ITutorService
    {
        private readonly string _csvFilePath = "Data/tutors.csv";

        public async Task<List<Tutor>> GetAllTutorsAsync()
        {
            return await ReadTutorsAsync();
        }

        public async Task<Tutor?> GetTutorByIdAsync(int id)
        {
            var tutors = await ReadTutorsAsync();
            return tutors.FirstOrDefault(t => t.Id == id);
        }

        public async Task AddTutorAsync(Tutor tutor)
        {
            var tutors = await ReadTutorsAsync();
            tutor.Id = tutors.Any() ? tutors.Max(t => t.Id) + 1 : 1;
            tutors.Add(tutor);
            await WriteTutorsAsync(tutors);
        }

        public async Task UpdateTutorAsync(Tutor tutor)
        {
            var tutors = await ReadTutorsAsync();
            var index = tutors.FindIndex(t => t.Id == tutor.Id);
            if (index >= 0)
            {
                tutors[index] = tutor;
                await WriteTutorsAsync(tutors);
            }
        }

        public async Task DeleteTutorAsync(int id)
        {
            var tutors = await ReadTutorsAsync();
            var tutor = tutors.FirstOrDefault(t => t.Id == id);
            if (tutor != null)
            {
                tutors.Remove(tutor);
                await WriteTutorsAsync(tutors);
            }
        }

        private async Task<List<Tutor>> ReadTutorsAsync()
        {
            var tutors = new List<Tutor>();
            if (!File.Exists(_csvFilePath))
                return tutors;

            var lines = await File.ReadAllLinesAsync(_csvFilePath);
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    tutors.Add(new Tutor
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1],
                        Email = parts[2],
                        ExperienceYears = int.Parse(parts[3])
                    });
                }
            }
            return tutors;
        }

        private async Task WriteTutorsAsync(List<Tutor> tutors)
        {
            var lines = new List<string> { "Id,Name,Email,ExperienceYears" };
            lines.AddRange(tutors.Select(t => $"{t.Id},{t.Name},{t.Email},{t.ExperienceYears}"));
            await File.WriteAllLinesAsync(_csvFilePath, lines);
        }

        public async Task<Tutor?> GetTutorAsync(int id)
        {
            return await GetTutorByIdAsync(id);
        }
    }
}