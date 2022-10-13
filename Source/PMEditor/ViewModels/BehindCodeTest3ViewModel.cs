using Common.Model;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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

namespace PMEditor.ViewModels
{
    public class BehindCodeTest3ViewModel : BindableBase
    {
        public string Text { get; set; } = "TEST3";

        public ObservableCollection<ImageManager> ListCollection { get; set; } = new ObservableCollection<ImageManager>();
        public ObservableCollection<DriveInfo> DriveCollection { get; set; } = new ObservableCollection<DriveInfo>();

        public ImageSource Source1 { get; set; } = ImageUtil.ToImageSource(ImageUtil.GetDriveIcon(DriveItemType.DriveUnknown));
        public ImageSource Source2 { get; set; } = ImageUtil.ToImageSource(ImageUtil.GetDriveIcon(DriveItemType.DriveNoRootDir));
        public ImageSource Source3 { get; set; } = ImageUtil.ToImageSource(ImageUtil.GetDriveIcon(DriveItemType.DriveRemovable));
        public ImageSource Source4 { get; set; } = ImageUtil.ToImageSource(ImageUtil.GetDriveIcon(DriveItemType.DriveFixed));
        public ImageSource Source5 { get; set; } = ImageUtil.ToImageSource(ImageUtil.GetIcon(31));
        public ImageSource Source6 { get; set; } = ImageUtil.ToImageSource(ImageUtil.GetDriveIcon(DriveItemType.DriveRemote));
        public ImageSource Source7 { get; set; } = ImageUtil.ToImageSource(ImageUtil.GetDriveIcon(DriveItemType.DriveCdrom));
        public ImageSource Source8 { get; set; } = ImageUtil.ToImageSource(ImageUtil.GetDriveIcon(DriveItemType.DriveRamdisk));
        public ImageSource Source9 { get; set; }

        public BehindCodeTest3ViewModel()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            DriveCollection.Clear();

            foreach (var drive in allDrives)
                DriveCollection.Add(drive);

            DriveInfoManager DriveInfoManager = new DriveInfoManager();
            DriveInfoManager.driveItemType = DriveItemType.DriveCdrom;

            Source9 = ImageUtil.ToImageSource(DriveInfoManager.driveItemType.Icon());
        }
        private DelegateCommand<string> _TestCommand;
        public DelegateCommand<string> TestCommand => _TestCommand ?? (_TestCommand = new DelegateCommand<string>(ExecuteTestCommand));
        void ExecuteTestCommand(string param)
        {
            ListCollection.Clear();
            int searchIndex = 400;

            switch (param)
            {
               
                case "ShellIconsLib":
                    {
                        for (int i = 0; i < searchIndex; i++)
                        {
                            try
                            {
                                ListCollection.Add(new ImageManager() { Source = ImageUtil.ToImageSource(ImageUtil.GetIcon(i)), index = i });
                            }
                            catch (Exception e)
                            {
                                //ListCollection.Add(new ImageManager() { Source = ImageUtil.ToImageSource(ImageUtil.GetIcon(i, ImageUtil.ImageredIconsLib)), index = i });
                            }
                        }
                        break;
                    }
                case "ImageredIconsLib":
                    {
                        for (int i = 0; i < searchIndex; i++)
                        {
                            try
                            {
                                ListCollection.Add(new ImageManager() { Source = ImageUtil.ToImageSource(ImageUtil.GetIcon(i)), index = i });
                            }
                            catch (Exception e)
                            {
                                //ListCollection.Add(new ImageManager() { Source = ImageUtil.ToImageSource(ImageUtil.GetIcon(i, ImageUtil.ImageredIconsLib)), index = i });
                            }
                        }
                        break;
                    }
                case "DDOResIconsLib":
                    {
                        for (int i = 0; i < searchIndex; i++)
                        {
                            try
                            {
                                ListCollection.Add(new ImageManager() { Source = ImageUtil.ToImageSource(ImageUtil.GetIcon(i)), index = i });
                            }
                            catch (Exception e)
                            {
                                //ListCollection.Add(new ImageManager() { Source = ImageUtil.ToImageSource(ImageUtil.GetIcon(i, ImageUtil.ImageredIconsLib)), index = i });
                            }
                        }
                        break;
                    }
                case "morIconsLib":
                    {
                        for (int i = 0; i < searchIndex; i++)
                        {
                            try
                            {
                                ListCollection.Add(new ImageManager() { Source = ImageUtil.ToImageSource(ImageUtil.GetIcon(i)), index = i });
                            }
                            catch (Exception e)
                            {
                                //ListCollection.Add(new ImageManager() { Source = ImageUtil.ToImageSource(ImageUtil.GetIcon(i, ImageUtil.ImageredIconsLib)), index = i });
                            }
                        }
                        break;
                    }

                case "GetDrive":
                    {
                        DriveInfo[] allDrives = DriveInfo.GetDrives();
                        DriveCollection.Clear();

                        foreach (var drive in allDrives)
                            DriveCollection.Add(drive);

                        break;
                    }

                default:
                    break;

            }
        }

    }

    public class ImageManager : BindableBase
    {
        public ImageSource Source { get; set; }
        public int index { get; set; } = -1;
    }

    public class DriveInfoManager : BindableBase
    {
        public DriveItemType driveItemType { get; set; } = DriveItemType.DriveUnknown;
    }
}
