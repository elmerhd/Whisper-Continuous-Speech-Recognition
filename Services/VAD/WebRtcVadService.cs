using System;
using System.Collections.Generic;
using System.Text;
using WebRtcVadSharp;

namespace Whisper_Continuous_Speech_Recognition.Services.VAD
{
    public class WebRtcVadService : IVadService
    {
        private readonly WebRtcVad vad = new();

        public bool IsSpeech(byte[] frame)
        {
            return vad.HasSpeech(
                frame,
                SampleRate.Is16kHz,
                FrameLength.Is20ms);
        }
    }
}
