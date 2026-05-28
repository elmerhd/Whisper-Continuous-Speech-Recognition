using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using Whisper_Continuous_Speech_Recognition.Services.Audio;
using Whisper_Continuous_Speech_Recognition.Services.Speech;
using Whisper_Continuous_Speech_Recognition.Services.SpeechPipeline;
using Whisper_Continuous_Speech_Recognition.Services.VAD;
using Whisper_Continuous_Speech_Recognition.Services.Whisper;
using Whisper_Continuous_Speech_Recognition.ViewModels;

namespace Whisper_Continuous_Speech_Recognition
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }
        protected override async void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddSingleton<IAudioCaptureService, NAudioCaptureService>();
            services.AddSingleton<IVadService, WebRtcVadService>();
            services.AddSingleton<IWhisperService, WhisperService>();

            services.AddSingleton<SpeechPipelineService>();
            services.AddSingleton<IRealtimeSpeechService, RealtimeSpeechService>();

            services.AddSingleton<MainViewModel>();

            Services = services.BuildServiceProvider();

            var whisperService = Services.GetRequiredService<IWhisperService>();

            await whisperService.InitializeAsync();

            var window = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };

            window.Show();
        }
    }

}
