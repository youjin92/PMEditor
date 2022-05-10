using Common.IService;
using PMEditor.SampleModule.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PMEditor.SampleModule.ViewModels
{
    public class ProblemViewModel : BindableBase
    {
        ISolutionManager SolutionManager ;
        private string answer = "";
        private int order = 1;

        public Visibility AnswerBorderVisibility { get; set; } = Visibility.Collapsed;

        public ProblemViewModel(ISolutionManager _SolutionManager)
        {
            SolutionManager = _SolutionManager;
        }

        private DelegateCommand<object> _LoadedCommand;
        public DelegateCommand<object> LoadedCommand =>
            _LoadedCommand ?? (_LoadedCommand = new DelegateCommand<object>(ExecuteLoadedCommand));
        void ExecuteLoadedCommand(object parameter)
        {
            ProblemView problemView = (parameter as System.Windows.RoutedEventArgs).Source as ProblemView;

            if (problemView != null)
            {
                answer = problemView.Answer;
                order = problemView.Order;
            }

        }

        private DelegateCommand<object> _ClickCommand;
        public DelegateCommand<object> ClickCommand =>
            _ClickCommand ?? (_ClickCommand = new DelegateCommand<object>(ExecuteClickCommand));
        void ExecuteClickCommand(object parameter)
        {
            if (parameter.ToString() == answer)
            {
                AnswerBorderVisibility = Visibility.Visible;

                switch (order)
                {
                    case 1:
                        SolutionManager.Solution.Is1stSolved = true;
                        break;

                    case 2:
                        SolutionManager.Solution.Is2ndSolved = true;
                        break;

                    case 3:
                        SolutionManager.Solution.Is3thSolved = true;
                        break;
                    case 4:
                        SolutionManager.Solution.Is4thSolved = true;
                        break;
                    case 5:
                        SolutionManager.Solution.Is5thSolved = true;
                        break;
                    case 6:
                        SolutionManager.Solution.Is6thSolved = true;
                        break;

                    default:
                        break;
                }
            }

            if(SolutionManager.Solution.Is1stSolved && SolutionManager.Solution.Is2ndSolved && SolutionManager.Solution.Is3thSolved &&
                SolutionManager.Solution.Is4thSolved && SolutionManager.Solution.Is5thSolved && SolutionManager.Solution.Is6thSolved)
                SolutionManager.Solution.IsEventFire = true;
        }
    }
}
