using Whisper_Continuous_Speech_Recognition.Helpers;
using Whisper_Continuous_Speech_Recognition.Services.Speech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Whisper_Continuous_Speech_Recognition.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IRealtimeSpeechService speechService;

        private string transcript = "";

        public string Transcript
        {
            get => transcript;
            set
            {
                transcript = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public MainViewModel(IRealtimeSpeechService speechService)
        {
            this.speechService = speechService;

            speechService.OnTranscript += text =>
            {
                Transcript += text + Environment.NewLine;
            };

            StartCommand = new RelayCommand(_ => speechService.Start());
            StopCommand = new RelayCommand(_ => speechService.Stop());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
