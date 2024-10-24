using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal abstract class Marker {
	public string Name { get; set; }
	public double Lat { get; set; }
	public double Lng { get; set; }
	public string Description { get; set; }
	public string Icon { get; set; }
}
