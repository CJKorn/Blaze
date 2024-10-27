using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class JsonReadWrite : IReadWrite {
    private JsonSerializerOptions GetSerializerOptions() {
        var options = new JsonSerializerOptions {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        return options;
    }

    public void SerializeFile<T>(List<T> data, string filePath) {
        var options = GetSerializerOptions();
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, json);
    }

    public string SerializeString<T>(List<T> data) {
        var options = GetSerializerOptions();
        return JsonSerializer.Serialize(data, options);
    }

    public List<T> DeserializeFile<T>(string filePath) {
        if (!File.Exists(filePath)) {
            return new List<T>();
        }

        string json = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(json)) {
            return new List<T>();
        }

        return DeserializeString<T>(json);
    }

    public List<T> DeserializeString<T>(string json) {
        if (string.IsNullOrEmpty(json)) {
            return new List<T>();
        }

        try {
            var options = GetSerializerOptions();
            var jsonArray = JsonDocument.Parse(json).RootElement;
            var result = new List<T>();

            foreach (var element in jsonArray.EnumerateArray()) {
                var type = element.GetProperty("$type").GetString();
                switch (type) {
                    case "mono":
                        result.Add((T)(object)JsonSerializer.Deserialize<MonoReport>(element.GetRawText(), options));
                        break;
                    case "poly":
                        result.Add((T)(object)JsonSerializer.Deserialize<PolyReport>(element.GetRawText(), options));
                        break;
                    default:
                        throw new NotSupportedException($"Type {type} is not supported");
                }
            }

            return result;
        }
        catch (JsonException ex) {
            return new List<T>();
        }
    }
}
