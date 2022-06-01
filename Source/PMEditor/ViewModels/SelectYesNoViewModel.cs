using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMEditor.ViewModels
{
    public class SelectYesNoViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public SelectYesNoViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private DelegateCommand _TestCommand;
        public DelegateCommand TestCommand =>_TestCommand ?? (_TestCommand = new DelegateCommand(ExecuteTestCommand));
        void ExecuteTestCommand()
        {
            _regionManager.RequestNavigate("SelectRegion", "Problem1");
        }
    }
}
