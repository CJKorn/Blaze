using GoogleMapsComponents.Maps;
using System.Text.Json.Serialization;

[JsonDerivedType(typeof(PolyReport), "poly")] // Use "poly" as the type discriminator for Poly
public class PolyReport : Report {

	public List<LatLngLiteral> LatLngLiterals { get; set; }

    public PolyReport() { }

    public PolyReport(string name, string description, List<LatLngLiteral> latLngLiterals, Category cat) : base(name, description, cat) {
        LatLngLiterals = latLngLiterals;
        //Icon = icon;
        //Cat = cat;
    }
}