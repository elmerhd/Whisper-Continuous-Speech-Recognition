using Whisper_Continuous_Speech_Recognition.Services.SpeechPipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whisper_Continuous_Speech_Recognition.Services.Speech
{
    public class RealtimeSpeechService : IRealtimeSpeechService
    {
        private readonly SpeechPipelineService pipeline;

        public event Action<string>? OnTranscript;

        public RealtimeSpeechService(SpeechPipelineService pipeline)
        {
            this.pipeline = pipeline;

            pipeline.OnFinalTranscript += text =>
            {
                OnTranscript?.Invoke(text);
            };
        }

        public void Start() => pipeline.Start();
        public void Stop() => pipeline.Stop();
    }
}
