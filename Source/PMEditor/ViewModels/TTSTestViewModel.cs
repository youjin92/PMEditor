using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Speech;
using Microsoft.Speech.Synthesis;
using System.ComponentModel;

namespace PMEditor.ViewModels
{
    

    public class TTSTestViewModel : BindableBase
    {
        #region 필드
        SpeechSynthesizer tts = new SpeechSynthesizer();
        BackgroundWorker worker = new BackgroundWorker();

        #endregion

        #region 프로퍼티
        public string TextMessage { get; set; } = "Test";
        #endregion

        public TTSTestViewModel()
        {
            tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)");
            tts.SetOutputToDefaultAudioDevice();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);

        }

        #region 커멘드

        private DelegateCommand _TTSCommand;
        public DelegateCommand TTSCommand =>_TTSCommand ?? (_TTSCommand = new DelegateCommand(ExecuteTTSCommand));
        void ExecuteTTSCommand()
        {
            if(!worker.IsBusy)
                worker.RunWorkerAsync();
        }
        #endregion

        #region 함수

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextMessage))
                tts.Speak(TextMessage);
            else
                tts.Speak("텍스트 비었음.");
        }

        #endregion
    }
}
