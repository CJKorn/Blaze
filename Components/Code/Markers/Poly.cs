using GoogleMapsComponents.Maps;
using System.Text.Json.Serialization;

[JsonDerivedType(typeof(Poly), "poly")] // Use "poly" as the type discriminator for Poly
internal class Poly : Report {

	public List<LatLngLiteral> LatLngLiterals { get; set; }

	public Poly(string name, string description, List<LatLngLiteral> latLngLiterals, string icon, Category cat) : base(name, description, icon, cat) {
        LatLngLiterals = latLngLiterals;
		//Icon = icon;
		//Cat = cat;
	}
}