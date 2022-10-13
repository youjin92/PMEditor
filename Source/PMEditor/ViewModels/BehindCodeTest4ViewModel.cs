using Common.Model;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.Caching;
using Common;
using static Common.ShellApiWrapper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PMEditor.ViewModels
{
    public class BehindCodeTest4ViewModel : BindableBase
    {
        public string Text { get; set; } = "Text";
        public string Text2 { get; set; } = "Text2";
        public string DPText { get; set; } = "bbbaaabbb";

        public ImageSource Source { get; set; }

        private DelegateCommand<string> _TestCommand;
        public DelegateCommand<string> TestCommand => _TestCommand ?? (_TestCommand = new DelegateCommand<string>(ExecuteTestCommand));
        void ExecuteTestCommand(string param)
        {

            switch (param)
            {
                case "Convert":
                    {

                        break;
                    }

                case "Input":
                    {
                        DPText = Text2;
                        break;
                    }
                case "Test":
                    {
                        string a = "012345";
                        string b = $"{a[0]}{a[1]} {a[2]}{a[3]} {a[4]}{a[5]}";
                        DPText = b;
                        break;
                    }

                default:
                    break;

            }
        }

    }
}
