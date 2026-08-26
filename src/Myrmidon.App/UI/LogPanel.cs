using Bramble.Core;
using Myrmidon.Core;

namespace Myrmidon.App.Render;

public class LogPanel : GridPanel {

    private List<string> _messages = new List<string>();


    public LogPanel(Terminal terminal, Rect rect) : base(terminal, rect) {
    }
    
    public void AddEntry(string message) {
        _messages.Add(message);
    }

    public override void Render() {
        base.Render();
        if (_messages.Count > 0)
            RenderLog();
    }

    public void RenderLog() {
        for (int i = 0; i < _messages.Count; i++) {
            DrawText(new Vec(1, i), $"{_messages[i]}", "w");
        }
    }
}