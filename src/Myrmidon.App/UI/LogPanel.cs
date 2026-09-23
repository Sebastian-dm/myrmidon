using Bramble.Core;
using Myrmidon.App.Render;
using Myrmidon.Core;

namespace Myrmidon.App.UI;

public class LogPanel : GridPanel {

    private List<string> _messages = new List<string>();

    private int _linesVisible { get { return Height; }}


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
        int firstIndex = Math.Max(0, _messages.Count - _linesVisible);
        int numberOfMessages = Math.Min(_linesVisible, _messages.Count);
        var lastMessages = _messages.Slice(firstIndex, numberOfMessages);

        for (int i = 0; i < lastMessages.Count; i++) {
            DrawText(new Vec(1, i), $"{lastMessages[i]}", "w");
        }
    }
}