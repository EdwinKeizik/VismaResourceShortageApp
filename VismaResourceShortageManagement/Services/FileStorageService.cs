using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using VismaResourceShortageManagement.Models;

namespace VismaResourceShortageManagement.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _filePath;

        public FileStorageService(string filePath = "data/shortages.json")
        {
            _filePath = filePath;
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public List<Shortage> LoadShortages()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Shortage>();
            }

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    return new List<Shortage>();
                }
                List<Shortage>? shortages = JsonSerializer.Deserialize<List<Shortage>>(jsonString);
                return shortages ?? new List<Shortage>();
            }
            catch (JsonException)
            {
                return new List<Shortage>();
            }
            catch (IOException)
            {
                return new List<Shortage>();
            }
        }

        public void SaveShortages(List<Shortage> shortages)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(shortages, options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (JsonException)
            {

            }
            catch (IOException)
            {

            }
        }
    }
}
