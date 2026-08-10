using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Malison.Core
{
    //### bob: get rid of this?
    public class ColorPair
    {
        public TerminalColor Fore;
        public TerminalColor Back;

        public ColorPair(TerminalColor fore, TerminalColor back)
        {
            Fore = fore;
            Back = back;
        }
    }
}
