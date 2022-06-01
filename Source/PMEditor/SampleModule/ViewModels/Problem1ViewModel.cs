using Common.IService;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMEditor.SampleModule.ViewModels
{
    public class Problem1ViewModel : BindableBase
    {
        ISolutionManager SolutionManager;
        public Problem1ViewModel(ISolutionManager _SolutionManager)
        {
            SolutionManager = _SolutionManager;
        }

        private DelegateCommand _ClickCommand;
        public DelegateCommand ClickCommand =>
            _ClickCommand ?? (_ClickCommand = new DelegateCommand(ExecuteClickCommand));
        void ExecuteClickCommand()
        {
            SolutionManager.Solution.Is1stSolved = true;
            SolutionManager.Solution.IsEventFire = true;
        }
    }
}
