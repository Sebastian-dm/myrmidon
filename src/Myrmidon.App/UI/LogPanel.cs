using Bramble.Core;
using Myrmidon.App.Render;
using Myrmidon.Core;

namespace Myrmidon.App.UI;

public class LogPanel : GridPanel {

    private List<string> _messages = new List<string>();


    public LogPanel(TerminalRenderer terminal, Rect rect) : base(terminal, rect) {
        
    }
    
    public void AddEntry(string message) {
        _messages.Add(message);
    }

    public override void Draw() {
        base.Draw();
        FillBackground("black");
        if (_messages.Count > 0)
            RenderLog();
    }

    private void RenderLog() {
        for (int i = 0; i < _messages.Count; i++) {
            DrawText(new Vec(1, i), $"{_messages[i]}", "w");
        }
    }
}