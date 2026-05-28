using NAudio.Wave;

namespace Whisper_Continuous_Speech_Recognition.Services.Audio
{
    public interface IAudioCaptureService
    {
        event Action<byte[]> OnAudioFrame;
        void Start();
        void Stop();
    }
}