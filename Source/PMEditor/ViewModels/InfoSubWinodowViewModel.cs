using Common.PubSubEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Common.CommonManager;

namespace PMEditor.ViewModels
{
    public class InfoSubWinodowViewModel : BindableBase
    {
        public string Title { get; set; } = "Poketmon Property";
        public bool isTopMost { get; set; } = false;


        #region Field
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        
        #endregion

        public InfoSubWinodowViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }

        private DelegateCommand<string> _TopMostSettingCommand;
        public DelegateCommand<string> TopMostSettingCommand => _TopMostSettingCommand ?? (_TopMostSettingCommand = new DelegateCommand<string>(ExecuteOCRToggleCommand));
        void ExecuteOCRToggleCommand(string parameter)
        {

            switch (parameter)
            {
                case "Checked":
                    {
                        isTopMost = true;
                        break;
                    }
                case "UnChecked":
                    {
                        isTopMost = false;
                        break;
                    }
                default:
                    break;
            }

        }

        private DelegateCommand<object> _LoadedCommand;
        public DelegateCommand<object> LoadedCommand => _LoadedCommand ?? (_LoadedCommand = new DelegateCommand<object>(ExecuteLoadedCommand));
        void ExecuteLoadedCommand(object parameter)
        {
        }
    }
}
