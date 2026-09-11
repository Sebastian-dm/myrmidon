using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Components {
    public class Perceptible : Component {

        public float LightLevel { get; set; } = 0.0f;

        public bool Visible { get; set; } = true;
        public bool Explored { get; set; } = false;
    }
}
