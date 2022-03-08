using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Common
{
    public static class CommonManager
    {
        public static bool isAppWorking = true;
        public static ObservableCollection<Poketmon> PoketmonList = new ObservableCollection<Poketmon>();

        public static bool Toggle(bool origin)
        {
            if (origin)
                return false;
            else
                return true;
        }

        public static Visibility Toggle(Visibility origin, bool isToggleToCollapsed = true)
        {
            if (origin == Visibility.Visible)
            {
                if (isToggleToCollapsed)
                    return Visibility.Collapsed;
                else
                    return Visibility.Hidden;
            }
            else
                return Visibility.Visible;
        }

        public static class DispatcherService
        {
            public static void Invoke(Action action)
            {
                try
                {
                    Dispatcher dispatchObject = Application.Current != null ? Application.Current.Dispatcher : null;
                    if (dispatchObject == null || dispatchObject.CheckAccess())
                        action();
                    else
                        dispatchObject.Invoke(action);
                }
                catch {
                    Console.WriteLine("error");
                }

            }
        }
    }
}
