using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.App.Input {
    public enum AppCommand {
        ToggleStepMode,
        Pause,
        Resume,
        Quit
    }

    public sealed class AppCommandEventArgs(AppCommand command) : EventArgs {
        public AppCommand Command { get; } = command;
    }
}
