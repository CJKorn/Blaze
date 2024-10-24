using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class JsonReadWrite : IReadWrite {
    public void Serialize<T>(List<T> data, string filePath) {
        var options = new JsonSerializerOptions {
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            // Default serializer settings with polymorphism support
        };

        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, json);
    }

    public List<T> Deserialize<T>(string filePath) {
        if (!File.Exists(filePath)) {
            return new List<T>();
        }

        string json = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions {
            // Enable deserialization of polymorphic types
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        return JsonSerializer.Deserialize<List<T>>(json, options);
    }
}
