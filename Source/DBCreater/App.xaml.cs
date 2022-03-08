using DBCreater.Views;
using PMEditor.Views;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MainWindow = DBCreater.Views.MainWindow;

namespace DBCreater
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App 
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ExcelInfoResultView>();



        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var _regionManager = Container.Resolve<IRegionManager>();

            _regionManager.RequestNavigate("ExcelInfoResultViewRegion", "ExcelInfoResultView");
        }

        protected override void Initialize()
        {


            base.Initialize();

        }


        protected override void OnExit(ExitEventArgs e)
        {


            base.OnExit(e);
        }
    }
}
