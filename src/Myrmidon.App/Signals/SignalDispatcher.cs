using Myrmidon.Core.Signals;

namespace Myrmidon.App.Events;

public class SignalDispatcher
{
    private readonly SignalQueue _messageQueue;
    private readonly Action<ISignal>[] _handlers;

    public SignalDispatcher(SignalQueue messageQueue, params Action<ISignal>[] handlers)
    {
        _messageQueue = messageQueue;
        _handlers = handlers;
    }

    public void ProcessSignals()
    {
        while (_messageQueue.TryDequeue(out var message))
        {
            foreach (var handler in _handlers)
            {
                handler(message);
            }
        }
    }
}