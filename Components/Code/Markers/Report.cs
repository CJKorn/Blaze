using GoogleMapsComponents.Maps;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

[JsonPolymorphic] // Enable polymorphic serialization
[JsonDerivedType(typeof(Mono), "mono")] // Use "mono" as the type discriminator for Mono
[JsonDerivedType(typeof(Poly), "poly")] // Use "poly" as the type discriminator for Poly
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
    public string Icon { get; set; }
    public Category Cat { get; set; }

	public Report() {
        Name = "";
        Description = "";
        Icon = "";
    }

    public Report(string name, string description, string icon) {
        Name = name;
        Description = description;
        Icon = icon;
    }

    public Report(string name, string description, string icon, Category cat) {
		Name = name;
		Description = description;
		Icon = icon;
        Cat = cat;
	}

    public Report(string name, string description) {
        Name = name;
        Description = description;
        Icon = "";
    }

    public Report(string name) {
        Name = name;
        Description = "";
        Icon = "";
    }

    public override string ToString() {
        return $"{Name} {Description}";
    }
}


