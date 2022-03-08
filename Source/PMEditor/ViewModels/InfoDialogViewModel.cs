using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMEditor.ViewModels
{
    public class InfoDialogViewModel : BindableBase, IDialogAware
    {
        #region 프로퍼티
        public string Title { get; set; } = "Poketmon Property";
        public string TextString { get; set; } = "test";

        #endregion


        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            Console.WriteLine("CanCloseDialog");
            return true;
        }

        public void OnDialogClosed()
        {
            Console.WriteLine("OnDialogClosed");
            return;
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            TextString = parameters.GetValue<string>("TestKey");

        }
       
    }
}
