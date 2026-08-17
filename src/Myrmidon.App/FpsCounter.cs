using SDL3;

namespace Myrmidon.App;

public class FpsCounter {

    public int TargetFps { get; private set; }
    public double Fps { get; private set; }
    
    private ulong _lastTime = SDL.GetPerformanceCounter();
    private int _frameCount;
    private double _targetTime;
    private double _elapsedTime;

    public FpsCounter(int targetFps) {
        TargetFps = targetFps;
        _targetTime = 1000.0 / targetFps;
    }

    public double GetTickRemainderMs() {
        var remainder = (_targetTime - _elapsedTime);
        if (remainder < 0) return 0;
        return remainder;
    }

    public void SetTargetFps(int targetFps) {
        TargetFps = targetFps;
        _targetTime = 1000.0 / targetFps;
    }

    public void Update() {
        _frameCount++;
        var currentTime = SDL.GetPerformanceCounter();
        _elapsedTime = (currentTime - _lastTime) / (double)SDL.GetPerformanceFrequency();

        if (_elapsedTime < 0.1)
            return;
        
        Fps = _frameCount / _elapsedTime;
        _frameCount = 0;
        _lastTime = currentTime;
        //SDL.Log($"Elapsed, {string.Format("{0:F3}", _elapsedTime)}, Target: {string.Format("{0:F3}", _targetTime)}, Remainder: {string.Format("{0:F3}", GetTickRemainderMs())}");
    }
}
