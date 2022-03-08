using Common;
using Common.Capture;
using Common.OCR;
using Common.PubSubEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PMEditor.ViewModels
{
    public class SimpleImageViewModel : BindableBase
    {
        public SimpleImageViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #region 필드
        IEventAggregator _eventAggregator;
        string FileName;
        #endregion

        #region 프로퍼티
        public ImageSource ImageSource { get; set; } = new BitmapImage(new Uri(@"pack://application:,,,/Common;component/Images/PMIcon.png", UriKind.RelativeOrAbsolute));
        public Stretch SelectedItem { get; set; } = Stretch.None;
        public Visibility IsSliderPanelVisable { get; set; } = Visibility.Collapsed;
        public double SliderValue { get; set; } = 175;

        #endregion

        #region 커멘드
        private DelegateCommand _RefreshCommand;
        public DelegateCommand RefreshCommand =>_RefreshCommand ?? (_RefreshCommand = new DelegateCommand(ExecuteRefreshCommand));
        void ExecuteRefreshCommand()
        {
            _eventAggregator.GetEvent<CaptureAndContrastImageEvent>().Publish();

            string source_file = $"{FileManager.ImageRootPath}\\{FileName}";
            string dest_file = $"{FileManager.ImageRootPath}\\Temp.png";

            System.IO.File.Copy(source_file, dest_file, true);

            ImageSource = CustomBitmapHelper.BitmapImageFromFile(dest_file) as BitmapImage;
        }

        private DelegateCommand<string> loadedCommand;
        public DelegateCommand<string> LoadedCommand =>loadedCommand ?? (loadedCommand = new DelegateCommand<string>(ExecuteLoadedCommand));
        void ExecuteLoadedCommand(string param)
        {
            FileName = param;

            if (FileName == "contract.png")
                IsSliderPanelVisable = Visibility.Visible;
        }

        private DelegateCommand _ValueChangedCommand;
        public DelegateCommand ValueChangedCommand =>_ValueChangedCommand ?? (_ValueChangedCommand = new DelegateCommand(ExecuteValueChangedCommand));
        void ExecuteValueChangedCommand()
        {
            CapureManager.Cutoff = (int)SliderValue;
            ExecuteRefreshCommand();
        }
        #endregion

    }
}
