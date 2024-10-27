using GoogleMapsComponents.Maps;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

[JsonPolymorphic] // Enable polymorphic serialization
[JsonDerivedType(typeof(MonoReport), "mono")] // Use "mono" as the type discriminator for Mono
[JsonDerivedType(typeof(PolyReport), "poly")] // Use "poly" as the type discriminator for Poly
public abstract class Report {
	public enum Category {
		Default,
		Accident,
		Fire,
		Crime,
		PowerOutage,
		WaterOutage,
		Pollution,
		WildlifeIncident,
		Traffic,
		RoadClosure,
		PublicTransit,
		Flood,
		SevereWeather,
		Heatwave,
		PublicGathering,
		Construction
	}
	public string Name { get; set; }
    public string Description { get; set; }
    public Category Cat { get; set; }
    public DateTime ReportTime { get; set; }

	public Report() {
        Name = "";
        Description = "";
    }

    //constructor for the report with the category and no icon
    public Report(string name, string description, Category cat) {
        Name = name;
        Description = description;
        Cat = cat;
    }

    //constructor for the report with all components
    public Report(string name, string description, string icon, Category cat) {
		Name = name;
		Description = description;
        Cat = cat;
	}
    //to string method for the report, that prints out the Name and the Description of the report
    public override string ToString() {
        return $"{Name} {Description}";
    }
}


