using System;
using System.Collections.Generic;
using System.Text;

namespace Whisper_Continuous_Speech_Recognition.Services.Speech
{
    public interface IRealtimeSpeechService
    {
        event Action<string> OnTranscript;
        void Start();
        void Stop();
    }
}
