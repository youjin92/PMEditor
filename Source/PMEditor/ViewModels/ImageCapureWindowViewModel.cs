using Common.PubSubEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using static Common.CommonManager;

namespace PMEditor.ViewModels
{
    public class ImageCapureWindowViewModel : BindableBase
    {
        public string Title { get; set; } = "Poketmon Property";
        public bool isTopMost { get; set; } = false;


        #region Field
        private readonly IEventAggregator _eventAggregator;
        #endregion

        public ImageCapureWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

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
    }
}
