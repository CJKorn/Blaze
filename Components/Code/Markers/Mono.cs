using System.Text.Json.Serialization;

[JsonDerivedType(typeof(Mono), "mono")] // Use "mono" as the type discriminator for Mono
internal class Mono : Report {

	public double Lat { get; set; }
	public double Lng { get; set; }

	public Mono(string name, string description, double lat, double lng, string icon, Category cat) : base(name, description, icon, cat) {
		Lat = lat;
		Lng = lng;
		//Cat = cat;
	}
}
