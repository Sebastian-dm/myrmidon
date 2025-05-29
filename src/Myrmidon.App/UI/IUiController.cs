using Bramble.Core;
using Malison.Core;
using Malison.WinForms;
using Myrmidon.App.Rendering;
using Myrmidon.Core.GameState;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Myrmidon.App.UI {
    public interface IUiController {

        public string Title { get; set; }

        public Vec Size { get; set; }

        public void Quit();
    }
}
