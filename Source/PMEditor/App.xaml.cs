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
using System.Windows;
using System.Windows.Threading;

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
            //public static Thread thread;
            //thread = new Thread(() =>
            //{

            //    LoadingWindow w = new LoadingWindow();
            //    w.DataContext = new LoadingWindowViewModel();
            //    loadingWindow = w;
            //    w.Show();

            //    w.Closed += (sender2, e2) => w.Dispatcher.InvokeShutdown();

            //    Dispatcher.Run();
            //});

            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();

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
