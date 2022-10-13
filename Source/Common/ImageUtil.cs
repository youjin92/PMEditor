using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Common
{
    public static class ImageUtil
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ToImageSource(Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }

        public const string ShellIconsLib = @"C:\WINDOWS\System32\shell32.dll";
        public const string DDOResIconsLib = @"C:\WINDOWS\System32\DDORes.dll";
        public const string morIconsLib = @"C:\WINDOWS\System32\moricons.dll";

        private const string ImageresIconsLib = @"C:\WINDOWS\System32\imageres.dll";


        [DllImport("shell32.dll",EntryPoint = "ExtractIcon")]
        extern static IntPtr ExtractIcon( IntPtr hInst,string lpszExeFileName,int nIconIndex);

        public static Icon GetIcon(int index)
        {
            IntPtr Hicon = ExtractIcon(IntPtr.Zero, ImageresIconsLib, index);
            Icon icon = System.Drawing.Icon.FromHandle(Hicon);
            return icon;
        }

        public static Icon GetDriveIcon(DriveItemType type)
        {
            switch (type)
            {
                case DriveItemType.DriveUnknown:
                    return GetIcon((int)ImageresIcon.Unknown);

                case DriveItemType.DriveNoRootDir:
                    return GetIcon((int)ImageresIcon.NoRootdir);

                case DriveItemType.DriveRemovable:
                    return GetIcon((int)ImageresIcon.Removable);

                case DriveItemType.DriveFixed:
                    //추가조건(Main)
                    //return GetIcon((int)ImageresIcon.MainFixed);
                    return GetIcon((int)ImageresIcon.Fixed);

                case DriveItemType.DriveRemote:
                    return GetIcon((int)ImageresIcon.Remote);

                case DriveItemType.DriveCdrom:
                    return GetIcon((int)ImageresIcon.CD);

                case DriveItemType.DriveRamdisk:
                    return GetIcon((int)ImageresIcon.RAM);
            }

            return null;
        }

        public static Icon Icon(this DriveItemType driveItemType)
        {
            return GetDriveIcon(driveItemType);
        }
    }

    public enum DriveItemType
    {
        DriveUnknown = 0,	
	    DriveNoRootDir = 1,		
        DriveRemovable = 2,		
        DriveFixed = 3,			
        DriveRemote = 4,		
        DriveCdrom = 5,			
	    DriveRamdisk = 6,		
    }

    public enum ImageresIcon
    {
        Unknown = 70,
        NoRootdir = 26,
        Removable = 27,
        Fixed = 30,
        MainFixed = 31,
        Remote = 28,
        CD = 25,
        RAM = 29,
    }
}
