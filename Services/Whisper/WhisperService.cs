using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Whisper.net;
using Whisper.net.Ggml;

namespace Whisper_Continuous_Speech_Recognition.Services.Whisper
{
    public class WhisperService : IWhisperService
    {
        private WhisperProcessor? processor;
        private WhisperFactory? factory;

        public WhisperProcessor Processor =>
            processor ?? throw new Exception("Whisper not initialized.");

        public async Task InitializeAsync()
        {
            string modelPath = "ggml-base.bin";

            if (!File.Exists(modelPath))
            {
                using var modelStream = await WhisperGgmlDownloader.Default
                    .GetGgmlModelAsync(GgmlType.Base);

                using var fileWriter = File.OpenWrite(modelPath);

                await modelStream.CopyToAsync(fileWriter);
            }

            factory = WhisperFactory.FromPath(modelPath);

            processor = factory
                .CreateBuilder()
                .WithLanguage("en")
                .Build();
        }

        public async Task<string> TranscribeAsync(byte[] pcm)
        {
            using var ms = new MemoryStream();
            using var writer = new WaveFileWriter(ms, new WaveFormat(16000, 16, 1));

            writer.Write(pcm, 0, pcm.Length);
            writer.Flush();

            ms.Position = 0;

            string text = "";

            await foreach (var result in processor.ProcessAsync(ms))
            {
                text += result.Text;
            }

            return text.Trim();
        }
    }
}
