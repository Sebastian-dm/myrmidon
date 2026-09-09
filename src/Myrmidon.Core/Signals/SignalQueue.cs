using System.Collections.Concurrent;

namespace Myrmidon.Core.Signals;

public class SignalQueue
{
    private readonly ConcurrentQueue<ISignal> _signals = new();

    public void Enqueue(ISignal signal) => _signals.Enqueue(signal);
    public bool TryDequeue(out ISignal signal) => _signals.TryDequeue(out signal);
}