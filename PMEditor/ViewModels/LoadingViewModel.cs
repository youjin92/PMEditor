using Common.PubSubEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PMEditor.ViewModels
{
    public class LoadingViewModel : BindableBase
    {
        #region 필드
        IEventAggregator _eventAggregator;
        #endregion

        #region 프로퍼티
        public string LoadingText { get; set; } = "엑셀파일 정보 불러오는중...";
        public string Name { get; set; } = "PMEditor";
        public string Content { get; set; } = "Loading...";
        #endregion

        public LoadingViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<LoadingTextChangeEvent>().Subscribe(ExcuteReceiveItem);
        }

        private void ExcuteReceiveItem(EventParam obj)
        {
            LoadingText = obj.Item.ToString();
        }


    }

}
