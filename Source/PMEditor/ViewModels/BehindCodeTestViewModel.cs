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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PMEditor.ViewModels
{
    public class BehindCodeTestViewModel : BindableBase
    {
        public ObservableCollection<Persons> TreeViewList { get; set; } = new ObservableCollection<Persons>();


        public BehindCodeTestViewModel()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            TreeViewList.Add(new Persons()
            {
                Name = "john",
                Age = 11,
                Children = new ObservableCollection<Persons>() {
                new Persons() { Name = "jack", Age = 11 ,
                    Children = new ObservableCollection< Persons> (){ new Persons() { Name = "aaa", Age = 23, Children = new ObservableCollection<Persons>()}}}} 
            });
            TreeViewList.Add(new Persons()
            {
                Name = "kohohn",
                Age = 11,
                Children = new ObservableCollection<Persons>() {
                new Persons() { Name = "qack", Age = 11 } }
            });
        }

        private DelegateCommand<string> _TestCommand;
        public DelegateCommand<string> TestCommand => _TestCommand ?? (_TestCommand = new DelegateCommand<string>(ExecuteTestCommand));
        void ExecuteTestCommand(string param)
        {
            string originPath = @"D:\KakaoTalk_20181103_204644132.jpg";
            string targetPath = @"D:\KakaoTalk_20181103_2046441323333.jpg";

            byte[] Buffer = new byte[1024];

            string path = @"C:\Folder1\Folder2\Folder3\Folder4\";
            string folderName = "Test.txt";
            string newPath = "";

            string strCheckFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));

            switch (param)
            {
                case "PreviewPath":
                    newPath = Path.GetFullPath(Path.Combine(path, @"..\"));
                    break;
                case "CombinePath":
                    newPath = Path.GetFullPath(Path.Combine(path, folderName));

                    DirectoryInfo directory = new DirectoryInfo(@"C:\");

                    DirectoryInfo[] directorys = directory.GetDirectories();
                    DirectoryInfo[] filteredDir = directorys.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();

                    FileInfo[] files = directory.GetFiles();
                    FileInfo[] filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();

                    var test = FileHelper.GetFileDirPath(@"C:\Users\jyou_estsecurity\Documents\repos\MaterialDesignInXamlToolkit-master (1).zip");
                    var Newtest = Path.GetFullPath(Path.Combine(test, @"Best.txt"));

                    var test11 = FileHelper.GetDirInfoList(@"C:\Users\jyou_estsecurity\Documents\repos\PMEditor_Git\Source");

                    DriveInfo[] driveInfos = FileHelper.GetDriveInfos();

                    var testbool = FileHelper.IsDrive(driveInfos[0].Name);


                    break;

                case "ReadAllBytes":
                    Buffer = File.ReadAllBytes(originPath);
                    break;
                case "WriteAllBytes":
                    File.WriteAllBytes(targetPath, Buffer);
                    break;

                case "INI만들기":
                    //var booleee = CreateIni("Test");

                    string e = @"C:\Users\jyou_estsecurity\Documents\repos\PMEditor_Git\Source\PMEditor\bin\Debug\INI\새 폴더";
                    string Fullpath = e += "\\" + "test1" + ".ini";
                    if (!System.IO.File.Exists(Fullpath))
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Fullpath, true, Encoding.GetEncoding(949)))
                        {
                            sw.Write("\r\n");
                            sw.Flush();
                            sw.Close();
                        }
                    }
                    break;

                case "INI쓰기":
            

                    if (setIni("Test_Info", "Test", "1231231231", strCheckFolder + "\\INI\\Test.ini"))
                    {
                    }
                    break;

                case "INI읽기":

                    var Text1 = getIni("Test_Info", "Test", "", strCheckFolder + "\\INI\\Test.ini");
                    break;
                case "JsonConvert":
                    JsonTest jsonTest = new JsonTest() { Age = 1, Name = "test", Test = new PersonB() { Name = "유진", Age = 31 } };

                    string jsonTxt = JsonConvert.SerializeObject(jsonTest);

                    JsonTest jsonTest1 = JsonConvert.DeserializeObject<JsonTest>(jsonTxt);

                    PersonB a = JsonConvert.DeserializeObject<PersonB>(JsonConvert.SerializeObject(jsonTest1.Test));

                    break;
                case "FileImage추출":
                    string path22 = @"D:\속도테스트2 - 복사본 (2).txt";
                    Icon icn = Icon.ExtractAssociatedIcon(path22);
                    img = Image.FromHbitmap(icn.ToBitmap().GetHbitmap());

                    break;
                case "FileImage설정":
                    using (var memory = new MemoryStream())
                    {
                        img.Save(memory, ImageFormat.Png);
                        memory.Position = 0;

                        var bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memory;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();

                        btmImage= bitmapImage;
                    }

                    ImageSource = btmImage;

                    break;
                default:
                    break;
            }
        }
        Image img;
        BitmapImage btmImage;
        public ImageSource ImageSource { get; set; }

        #region 함수
        //INIFile 만들기...
        private Boolean CreateIni(string strFileName)
        {
            try
            {
                string strCheckFolder = "";

                strCheckFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
                strCheckFolder += "\\INI";
                if (!System.IO.Directory.Exists(strCheckFolder))
                {
                    System.IO.Directory.CreateDirectory(strCheckFolder);

                }

                strCheckFolder += "\\" + strFileName + ".ini";
                if (!System.IO.File.Exists(strCheckFolder))
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(strCheckFolder, true, Encoding.GetEncoding(949)))
                    {
                        sw.Write("\r\n");
                        sw.Flush();
                        sw.Close();
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
            return true;
        }

        //INIFile 읽어오기...
        private string getIni(string IpAppName, string IpKeyName, string lpDefalut, string filePath)
        {
            string inifile = filePath;    //Path + File

            try
            {
                StringBuilder result = new StringBuilder(255);
                GetPrivateProfileString(IpAppName, IpKeyName, lpDefalut, result, result.Capacity, inifile);

                return result.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "실패";
            }
        }

        //INIFile 쓰기...
        private Boolean setIni(string IpAppName, string IpKeyName, string IpValue, string filePath)
        {
            try
            {
                string inifile = filePath;  //Path + File
                WritePrivateProfileString(IpAppName, IpKeyName, IpValue, inifile);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        [DllImport("KERNEL32.DLL")]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("KERNEL32.DLL")]
        private static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32.dll")]
        static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        #endregion

        private DelegateCommand<object> _ExpandedCommand;
        public DelegateCommand<object> ExpandedCommand => _ExpandedCommand ?? (_ExpandedCommand = new DelegateCommand<object>(ExecuteExpandedCommand));
        void ExecuteExpandedCommand(object param)
        {
            Console.WriteLine("ExecuteExpandedCommand");
        }

        private DelegateCommand<object> _KeyDownCommand;
        public DelegateCommand<object> KeyDownCommand => _KeyDownCommand ?? (_KeyDownCommand = new DelegateCommand<object>(ExecuteKeyDownCommand));
        void ExecuteKeyDownCommand(object parameter)
        {
            var args = parameter as System.Windows.Input.KeyEventArgs;

            if (args.Key == Key.Oem5)       //\
            { }
       

        }
    }
    public class JsonTest : BindableBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public object Test { get; set; }

    }

    public class PersonB : BindableBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class Persons : BindableBase
    {
        public string  Name { get; set; }
        public int Age { get; set; }
        public ObservableCollection<Persons> Children { get; set; } = new ObservableCollection<Persons>();
    }

    public static class FileHelper
    {
        public static void CreateDirectory(string filePath)
        {
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        }

        public static string GetFileName(string filePath) { return Path.GetFileName(filePath); }
        public static string GetFileNameWithoutExt(string filePath) { return Path.GetFileNameWithoutExtension(filePath); }
        public static string GetFileDirPath(string filePath) { return Path.GetDirectoryName(filePath); }
        public static string GetFileExt(string filePath) { return Path.GetExtension(filePath); }

        public static bool IsDiractory(string path)
        {
            if (!File.Exists(path))
            {
                //File.Create(path);
                return false;
            }

            FileAttributes attributes = File.GetAttributes(path);

            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                return true;
            else
                return false;
        }

        public static bool IsDrive(string path)
        {
            DriveInfo[] driveInfos = GetDriveInfos();

            foreach (DriveInfo driveinfo in driveInfos)
            {
                if (path == driveinfo.Name)
                    return true;
            }

            return false;
        }

        public static DriveInfo[] GetDriveInfos()
        {
            return DriveInfo.GetDrives();
        }

        public static string GetpreviousPath(string path)
        {
            return Path.GetFullPath(Path.Combine(path, @"..\"));
        }

        public static string GetCombinePath(string path, string file)
        {
            return Path.GetFullPath(Path.Combine(path, file));
        }

        public static FileInfo[] GetFileInfoList(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            return directory.GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
        }

        public static DirectoryInfo[] GetDirInfoList(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            return directory.GetDirectories().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
        }

        public static bool Copy(string sourceFileName, string destFileName, bool overWrite = true)
        {
            try
            {
                File.Copy(sourceFileName, destFileName, overWrite);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public static bool Move(string sourceFileName, string destFileName, bool overWrite = true)
        {
            if (File.Exists(destFileName))
            {
                if (overWrite)
                {
                    File.Delete(destFileName);
                }
                else
                {
                    Console.WriteLine("destFile is already existed!!!!!");
                    return false;
                }
            }

            try
            {
                File.Move(sourceFileName, destFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public static bool Rename(string sourceFileName, string NewName)
        {
            string NewPath = Path.GetFullPath(Path.Combine(GetFileDirPath(sourceFileName), NewName));

            try
            {
                Move(sourceFileName, NewPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
    }
}
