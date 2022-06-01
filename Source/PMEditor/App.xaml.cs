using Common;
using Common.Excel;
using PMEditor.ViewModels;
using PMEditor.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static Common.CommonManager;

namespace PMEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        //public static LoadingWindow loadingWindow = null;
        //public static Thread thread;

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PropertyView>();
            containerRegistry.RegisterForNavigation<ExcelInfoResultView>();
            containerRegistry.RegisterForNavigation<LoadingView>();
            containerRegistry.RegisterDialog<InfoDialogView>();

        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var _regionManager = Container.Resolve<IRegionManager>();

            _regionManager.RequestNavigate("LoadingViewRegion", "LoadingView");
            _regionManager.RequestNavigate("PropertyViewRegion", "PropertyView");
            _regionManager.RequestNavigate("ExcelInfoResultViewRegion", "ExcelInfoResultView");

            
        }

        protected override void Initialize()
        {

            bool isNeedInstallFile = false;

            if (!FileManager.CheckInstalledApplications("Microsoft Server Speech Platform Runtime (x86)"))
            {
                isNeedInstallFile = true;
            }
            if (!FileManager.CheckInstalledApplications("Microsoft Speech Platform SDK (x86) v11.0"))
            {
                isNeedInstallFile = true;
            }
            if (!FileManager.CheckInstalledApplications("Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)"))
            {
                isNeedInstallFile = true;
            }

            if (isNeedInstallFile)
            {
                Thread thread;
                string LoadingText = "Test";

                thread = new Thread(() =>
                {
                    LoadingWindow w = new LoadingWindow();
                    w.DataContext = new LoadingWindowViewModel();
                    (w.DataContext as LoadingWindowViewModel).LoadingText = LoadingText;
                    w.Closed += (sender2, e2) => w.Dispatcher.InvokeShutdown();
                    w.Show();

                    Dispatcher.Run();
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                if (!FileManager.CheckInstalledApplications("Microsoft Server Speech Platform Runtime (x86)"))
                {
                    LoadingText = "Microsoft Server Speech Platform Runtime (x86) 설치중...";
                    FileManager.InstallApplication("SpeechPlatformRuntime.msi", true);
                }
                if (!FileManager.CheckInstalledApplications("Microsoft Speech Platform SDK (x86) v11.0"))
                {
                    LoadingText = "Microsoft Speech Platform SDK(x86) v11.0 설치중...";
                    FileManager.InstallApplication("MicrosoftSpeechPlatformSDK.msi", true);
                }
                if (!FileManager.CheckInstalledApplications("Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)"))
                {
                    LoadingText = "Microsoft Server Speech Text to Speech Voice(ko - KR, Heami) 설치중...";
                    FileManager.InstallApplication("MSSpeech_TTS_ko-KR_Heami.msi", true);
                }

                thread.Abort();
            }

            base.Initialize();
        }


        protected override void OnExit(ExitEventArgs e)
        {
            CommonManager.isAppWorking = false;
            uint processId = 0;
            try
            {
                GetWindowThreadProcessId(new IntPtr(ExcelManager.currentexcel.Hwnd), out processId);
            }
            catch
            {
                Console.WriteLine("error");
            }
           

            if (processId != 0)
            {
                System.Diagnostics.Process excelProcess = System.Diagnostics.Process.GetProcessById((int)processId);
                excelProcess.CloseMainWindow();
                excelProcess.Refresh();
                excelProcess.Kill();
            }

            base.OnExit(e);
        }
    }
}
