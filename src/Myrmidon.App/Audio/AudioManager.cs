using Myrmidon.Core.Signals;
using SDL3;

namespace Myrmidon.App.Audio;


public class AudioManager
{
    private const string AssetsFolder = "../../../../../assets";


    private nint _wavData;
    private uint _wavDataLength;
    private nint _stream;

    public void PlaySound(string id)
    {
        Cleanup();
        var wavPath = $"{AssetsFolder}/sounds/{id}.wav";
        
        if (!SDL.LoadWAV(wavPath, out var spec, out _wavData, out _wavDataLength)) {
            throw new InvalidOperationException($"Couldn't load .wav file: {SDL.GetError()}");
        }
        
        _stream = SDL.OpenAudioDeviceStream(SDL.AudioDeviceDefaultPlayback, in spec, null, IntPtr.Zero);
        if (_stream == IntPtr.Zero) {
            throw new InvalidOperationException($"Couldn't create audio stream: {SDL.GetError()}");
        }

        SDL.ResumeAudioStreamDevice(_stream);
        SDL.PutAudioStreamData(_stream, _wavData, (int)_wavDataLength);
    }
    
    
    public void Tick()
    {
        // if (SDL.GetAudioStreamQueued(_stream) < (int)_wavDataLength) {
        //     SDL.PutAudioStreamData(_stream, _wavData, (int)_wavDataLength);
        // }
    }
    
    
    
    public void HandleSignal(ISignal signal) {
        if (signal is SoundSignal sound)
            PlaySound(sound.id);
    }
    
    
    private void Cleanup()
    {
        if (_stream != IntPtr.Zero) {
            SDL.DestroyAudioStream(_stream);
            _stream = IntPtr.Zero;
        }

        if (_wavData != IntPtr.Zero) {
            SDL.Free(_wavData);
            _wavData = IntPtr.Zero;
            _wavDataLength = 0;
        }
    }
}