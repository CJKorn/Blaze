using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
internal static class SessionData {
	public static bool signedIn { get; set; }
	public static bool hosting { get; set; }
	public static string myIP { get; set; }
	public static string IP { get; set; }
	public static string Key { get; set; }
	public static double Lat { get; set; }
	public static double Lng { get; set; }
	public static int Zoom { get; set; }
	public static double ClickedLat { get; set; }
	public static double ClickedLng { get; set; }
}
