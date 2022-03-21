using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Speech;
using Microsoft.Speech.Synthesis;
using System.ComponentModel;
using Common;

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

        public string FileName { get; set; }
        public string Result { get; set; } = "Not Yet";

        public string FileName2 { get; set; }
        public string Result2 { get; set; } = "Not Yet";

        public string FileOutputText { get; set; } = "FileOutputText";
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

        private DelegateCommand _SearchInstalledFileCommand;
        public DelegateCommand SearchInstalledFileCommand =>_SearchInstalledFileCommand ?? (_SearchInstalledFileCommand = new DelegateCommand(ExecuteSearchInstalledFileCommand));
        void ExecuteSearchInstalledFileCommand()
        {
            //"Microsoft Server Speech Platform Runtime (x86)"
            //"Microsoft Speech Platform SDK (x86) v11.0"
            //"Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)"
            bool result = FileManager.CheckInstalledApplications(FileName);

            if (result)
                Result = "Success ( " + FileName + " )";
            else
                Result = "Fail ( " + FileName + " )"; ;
        }

        private DelegateCommand _InstallFileCommand;
        public DelegateCommand InstallFileCommand =>_InstallFileCommand ?? (_InstallFileCommand = new DelegateCommand(ExecuteInstallFileCommand));
        void ExecuteInstallFileCommand()
        {
            FileManager.InstallApplication("MicrosoftSpeechPlatformSDK.msi", true);
        }

        private DelegateCommand _DropCommand;
        public DelegateCommand DropCommand => _DropCommand ?? (_DropCommand = new DelegateCommand(ExecuteDropCommand));
        void ExecuteDropCommand()
        {
            Console.WriteLine("ExecuteDropCommand");
        }

        private DelegateCommand _DragEnterCommand;
        public DelegateCommand DragEnterCommand => _DragEnterCommand ?? (_DragEnterCommand = new DelegateCommand(ExecuteDragEnterCommand));
        void ExecuteDragEnterCommand()
        {
            Console.WriteLine("ExecuteDragEnterCommand");
        }

        private DelegateCommand _DragLeaveCommand;
        public DelegateCommand DragLeaveCommand => _DragLeaveCommand ?? (_DragLeaveCommand = new DelegateCommand(ExecuteDragLeaveCommand));
        void ExecuteDragLeaveCommand()
        {
            Console.WriteLine("ExecuteDragLeaveCommand");
        }

        private DelegateCommand _LoadedCommand;
        public DelegateCommand LoadedCommand => _LoadedCommand ?? (_LoadedCommand = new DelegateCommand(ExecuteLoadedCommand));
        void ExecuteLoadedCommand()
        {
            Console.WriteLine("ExecuteLoadedCommand");
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
