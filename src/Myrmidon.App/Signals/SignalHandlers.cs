using Myrmidon.App.Render;
using Myrmidon.Core.Signals;

namespace Myrmidon.App.Events;

public class SignalHandlers {
    private readonly LogPanel _logPanel;
    
    public SignalHandlers(LogPanel logPanel)
    {
        _logPanel = logPanel;
    }
    
    // Handler for log messages
    public void HandleLogMessage(ISignal signal)
    {
        if (signal is LogSignal log)
            _logPanel.AddEntry(log.Text);
    }
}