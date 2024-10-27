using System.Text.Json.Serialization;

[JsonDerivedType(typeof(MonoReport), "mono")] // Use "mono" as the type discriminator for Mono
public class MonoReport : Report {

	public double Lat { get; set; }
	public double Lng { get; set; }

    public MonoReport() { }


    public MonoReport(string name, string description, double lat, double lng, Category cat) : base(name, description, cat) {
        Lat = lat;
        Lng = lng;

        //Cat = cat;
    }
}
