using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.IO;

namespace FileHandler
{
    public class JsonHandler
    {

        public static void Serialize(string filepath, object value)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(value, options);
            File.WriteAllText(filepath, jsonString);
        }

        public static T Deserialize<T>(string filepath)
        {
            string jsonString = File.ReadAllText(filepath);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
