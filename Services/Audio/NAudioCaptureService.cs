using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whisper_Continuous_Speech_Recognition.Services.Audio
{
    public class NAudioCaptureService : IAudioCaptureService
    {
        private WaveInEvent? waveIn;

        public event Action<byte[]>? OnAudioFrame;

        public void Start()
        {
            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 16, 1),
                BufferMilliseconds = 20
            };

            waveIn.DataAvailable += (s, e) =>
            {
                var frame = new byte[e.BytesRecorded];
                Buffer.BlockCopy(e.Buffer, 0, frame, 0, e.BytesRecorded);

                OnAudioFrame?.Invoke(frame);
            };

            waveIn.StartRecording();
        }

        public void Stop()
        {
            waveIn?.StopRecording();
            waveIn?.Dispose();
        }
    }
}
