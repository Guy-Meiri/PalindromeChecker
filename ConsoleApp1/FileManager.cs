using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class FileManager<T>
    {
        private readonly string _Path;

        public FileManager(string path)
        {
            _Path = path;
        }
        public async Task SaveToFileAsync(T obj)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(obj);
                await File.WriteAllTextAsync(_Path, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the file: {ex.Message}");
            }
        }

        public async Task<T> ReadFromFileAsync()
        {
            try
            {
                if (!File.Exists(_Path))
                {
                    throw new FileNotFoundException("The specified file does not exist.");
                }

                string jsonString = await File.ReadAllTextAsync(_Path);
                T obj = JsonSerializer.Deserialize<T>(jsonString);
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                return default;
            }
        }
    }
}
