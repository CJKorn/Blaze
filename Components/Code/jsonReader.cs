using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Swan.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class jsonReader {
	string json = JsonConvert.SerializeObject(new {
		foo = "bar",
		bar = "foo"
	});
}
