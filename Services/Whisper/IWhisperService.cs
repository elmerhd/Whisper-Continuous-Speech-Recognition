using System;
using System.Collections.Generic;
using System.Text;

namespace Whisper_Continuous_Speech_Recognition.Services.Whisper
{
    public interface IWhisperService
    {
        Task InitializeAsync();
        Task<string> TranscribeAsync(byte[] pcm);
    }
}
