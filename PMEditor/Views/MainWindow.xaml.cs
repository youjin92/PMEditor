using Common;
using Common.Capture;
using PMEditor.ViewModels;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PMEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IEventAggregator _eventAggregator;
        IRegionManager _regionManager;
        IDialogService _dialogService;

        public MainWindow(IEventAggregator eventAggregator, IRegionManager regionManager, IDialogService dialogService)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _dialogService = dialogService;

            System.IO.Directory.CreateDirectory(FileManager.ImageRootPath);
            System.IO.Directory.CreateDirectory(FileManager.DBPath);

            var vm = new MainWindowViewModel(this.rootgrid, _eventAggregator, _regionManager, _dialogService);
            this.DataContext = vm;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("PMEditor를 종료하시겠습니까?", "프로그램 종료", (MessageBoxButton)MessageBoxButtons.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
            {
            }
        }
    }
}
