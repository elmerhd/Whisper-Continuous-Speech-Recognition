using System;
using System.Collections.Generic;
using System.Text;

namespace Whisper_Continuous_Speech_Recognition.Services.VAD
{
    public interface IVadService
    {
        bool IsSpeech(byte[] frame);
    }
}
