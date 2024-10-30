using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
//this class is used to read and write to/from teh HSON files
public class JsonReadWrite : IReadWrite {
    private JsonSerializerOptions GetSerializerOptions() {
        var options = new JsonSerializerOptions {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        return options;
    }
    //the method below is used to serialize a file, by transforming the file into a json file.
    public void SerializeFile<T>(List<T> data, string filePath) {
        var options = GetSerializerOptions();
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, json);
    }
    //the method below is used to serialize a string, by converting a list of data into a json string
    public string SerializeString<T>(List<T> data) {
        var options = GetSerializerOptions();
        return JsonSerializer.Serialize(data, options);
    }
    //this method is used to deserialize a file, by converting the contents of the json file into a string, then converting the string itself back into a list
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
    //this method is used to deserialize a string, by converting the json string back into a List
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
