using System;
using System.Collections.Generic;
using System.IO;
using GoogleMapsComponents.Maps;
using NUnit.Framework;

[TestFixture]
public class JsonReadWriteTests {
    private JsonReadWrite _jsonReadWrite;

    [SetUp]
    public void SetUp() {
        _jsonReadWrite = new JsonReadWrite();
    }

    [Test]
    public void SerializeAndDeserializeMonoReport() {
        // Arrange
        var reports = new List<MonoReport>
        {
            new MonoReport("Test Mono", "Description Mono", 12.34, 56.78, Report.Category.Accident)
        };
        string filePath = "monoReport.json";

        // Act
        _jsonReadWrite.SerializeFile(reports, filePath);
        var deserializedReports = _jsonReadWrite.DeserializeFile<MonoReport>(filePath);

        // Assert
        Assert.Equals(1, deserializedReports.Count);
        Assert.Equals(reports[0].Name, deserializedReports[0].Name);
        Assert.Equals(reports[0].Description, deserializedReports[0].Description);
        Assert.Equals(reports[0].Lat, deserializedReports[0].Lat);
        Assert.Equals(reports[0].Lng, deserializedReports[0].Lng);
        Assert.Equals(reports[0].Cat, deserializedReports[0].Cat);

        // Clean up
        File.Delete(filePath);
    }

    [Test]
    public void SerializeAndDeserializePolyReport() {
        // Arrange
        var reports = new List<PolyReport>
        {
            new PolyReport("Test Poly", "Description Poly", new List<LatLngLiteral>
            {
                new LatLngLiteral { Lat = 12.34, Lng = 56.78 },
                new LatLngLiteral { Lat = 23.45, Lng = 67.89 }
            }, Report.Category.Fire)
        };
        string filePath = "polyReport.json";

        // Act
        _jsonReadWrite.SerializeFile(reports, filePath);
        var deserializedReports = _jsonReadWrite.DeserializeFile<PolyReport>(filePath);

        // Assert
        Assert.Equals(1, deserializedReports.Count);
        Assert.Equals(reports[0].Name, deserializedReports[0].Name);
        Assert.Equals(reports[0].Description, deserializedReports[0].Description);
        Assert.Equals(reports[0].LatLngLiterals.Count, deserializedReports[0].LatLngLiterals.Count);
        Assert.Equals(reports[0].LatLngLiterals[0].Lat, deserializedReports[0].LatLngLiterals[0].Lat);
        Assert.Equals(reports[0].LatLngLiterals[0].Lng, deserializedReports[0].LatLngLiterals[0].Lng);
        Assert.Equals(reports[0].Cat, deserializedReports[0].Cat);

        // Clean up
        File.Delete(filePath);
    }
}
