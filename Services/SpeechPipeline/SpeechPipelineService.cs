using Whisper_Continuous_Speech_Recognition.Services.Audio;
using Whisper_Continuous_Speech_Recognition.Services.VAD;
using Whisper_Continuous_Speech_Recognition.Services.Whisper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Whisper_Continuous_Speech_Recognition.Services.SpeechPipeline
{
    public class SpeechPipelineService
    {
        private readonly IAudioCaptureService audio;
        private readonly IVadService vad;
        private readonly IWhisperService whisper;

        private readonly List<byte> buffer = new();
        private readonly ConcurrentQueue<byte[]> queue = new();

        private bool speaking;
        private DateTime lastSpeech;

        private const int SilenceMs = 700;

        public event Action<string>? OnFinalTranscript;

        public SpeechPipelineService(
            IAudioCaptureService audio,
            IVadService vad,
            IWhisperService whisper)
        {
            this.audio = audio;
            this.vad = vad;
            this.whisper = whisper;

            audio.OnAudioFrame += Process;
        }

        public void Start() => audio.Start();
        public void Stop() => audio.Stop();

        private void Process(byte[] frame)
        {
            if (vad.IsSpeech(frame))
            {
                lastSpeech = DateTime.UtcNow;

                speaking = true;
                buffer.AddRange(frame);
            }
            else if (speaking &&
                     (DateTime.UtcNow - lastSpeech).TotalMilliseconds > SilenceMs)
            {
                speaking = false;

                var pcm = buffer.ToArray();
                buffer.Clear();

                _ = Task.Run(async () =>
                {
                    var text = await whisper.TranscribeAsync(pcm);

                    if (!string.IsNullOrWhiteSpace(text))
                        OnFinalTranscript?.Invoke(text);
                });
            }
        }
    }
}
