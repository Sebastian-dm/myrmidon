using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Parts;

internal class Identity {

    public string Name { get; set; } = "Unknown";

    public List<string> Groups { get; set; } = new List<string>();
}
