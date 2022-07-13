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

                case "ByteTest":
                    {
                        ObservableCollection<byte[]> bytesCollection = new ObservableCollection<byte[]>();
                        bytesCollection.Add(new byte[] {});
                        bytesCollection[0] = new byte[] { 1, 2, 3, 4, 5, 1, 12, 98, 3, 4, };
                        bytesCollection[1] = new byte[] { 2, 2, 3, 4, 5, 1, 12, 98, 3, 4, };
                        bytesCollection[2] = new byte[] { 3, 2, 3, 4, 5, 1, 12, 98, 3, 4, };

                        //List<byte[]> bytesCollection = new List<byte[]>();
                        //bytesCollection[0] = new byte[] { 1, 2, 3, 4, 5, 1, 12, 98, 3, 4, };
                        //bytesCollection[1] = new byte[] { 2, 2, 3, 4, 5, 1, 12, 98, 3, 4, };
                        //bytesCollection[2] = new byte[] { 3, 2, 3, 4, 5, 1, 12, 98, 3, 4, };

                        Console.WriteLine(bytesCollection);
                        foreach (var item in bytesCollection)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    }

                case "FileTest":
                    {
                        //DownloadManager DownloadManager = new DownloadManager();

                        //string TargetFilePath = @"C:\새 폴더\3gb.zip";

                        //FileStream fileStr = new FileStream(TargetFilePath, FileMode.Open, FileAccess.Read);

                        //long fileLength = fileStr.Length;

                        //long count = fileLength / 1024 + 1;

                        //long remain = fileLength % 1024;

                        //byte[] buffer = new byte[1024];

                        //int BlockIndex = 0;
                        //int BytesIndex = 0;

                        //long BlockSize = 1024 * 1024 * 1024;
                        //long TotalReadByte = 0;

                        ////CopyReady
                        //DownloadManager.Fileinfo = new FileInfo( TargetFilePath);

                        //long remain2 = 0;

                        //if (DownloadManager.Fileinfo.Length < 1024 * 1024 * 1024)
                        //    remain2 = DownloadManager.Fileinfo.Length;
                        //else
                        //    remain2 = DownloadManager.Fileinfo.Length % 1024 * 1024 * 1024;

                        //DownloadManager.BlockCollection.Clear();
                        //for (int index = 0; index < DownloadManager.BlockCount; index++)
                        //{
                        //    if (index == DownloadManager.BlockCount - 1)
                        //        DownloadManager.BlockCollection.Add(new Block(DownloadManager.Fileinfo.Name, index, remain2));
                        //    else
                        //        DownloadManager.BlockCollection.Add(new Block(DownloadManager.Fileinfo.Name, index, 1024 * 1024 * 1024));   //1GB 기준
                        //}
                        ////-----------



                        //BinaryReader reader = new BinaryReader(fileStr);
                        //for (long index = 0; index < count; index++)
                        //{
                        //    if (index == count - 1)
                        //        buffer = reader.ReadBytes((int)remain);
                        //    else
                        //        buffer = reader.ReadBytes(1024);

                        //    DownloadManager.BlockCollection[BlockIndex].bytesCollection[BytesIndex++] = buffer;

                        //    TotalReadByte += buffer.Count();

                        //    if (TotalReadByte == BlockSize ||
                        //        (TotalReadByte > BlockSize && TotalReadByte % BlockSize == 0))
                        //    {
                        //        BlockIndex++;
                        //        BytesIndex = 0;
                        //    }
                        //}
                        //reader.Close();

                        DownloadManager DownloadManager = new DownloadManager();

                        try
                        {
                            string TargetFilePath = @"C:\새 폴더\7gb.zip";

                            using (FileStream fsSource = new FileStream(TargetFilePath, FileMode.Open, FileAccess.Read))
                            {
                                long fileLength = fsSource.Length;

                                long count = fileLength / 1024 + 1;

                                long remain = fileLength % 1024;

                                byte[] buffer = new byte[1024];

                                int BlockIndex = 0;
                                int BytesIndex = 0;

                                long BlockSize = 1024 * 1024 * 1024;
                                long TotalReadByte = 0;

                                //CopyReady
                                DownloadManager.Fileinfo = new FileInfo(TargetFilePath);

                                long remain2 = 0;

                                if (DownloadManager.Fileinfo.Length < 1024 * 1024 * 1024)
                                    remain2 = DownloadManager.Fileinfo.Length;
                                else
                                    remain2 = DownloadManager.Fileinfo.Length % 1024 * 1024 * 1024;

                                DownloadManager.BlockCollection.Clear();
                                for (int index = 0; index < DownloadManager.BlockCount + 1; index++)
                                {
                                    if (index == DownloadManager.BlockCount - 1)
                                        DownloadManager.BlockCollection.Add(new Block(DownloadManager.Fileinfo.Name, index, 1024 * 1024 * 1024));
                                    else
                                        DownloadManager.BlockCollection.Add(new Block(DownloadManager.Fileinfo.Name, index, 1024 * 1024 * 1024));   //1GB 기준
                                }
                                //-----------

                                for (long index = 0; index < count; index++)
                                {
                                    if (BytesIndex == 462845)
                                    {

                                    }

                                    if (index == count - 1)
                                    {
                                        fsSource.Read(buffer, 0 , (int)remain);
                                    }
                                    else
                                    {
                                        fsSource.Read(buffer, 0 , 1024);
                                    }

                                    DownloadManager.BlockCollection[BlockIndex].bytesCollection[BytesIndex] = buffer;

                                    BytesIndex++;

                                    TotalReadByte += buffer.Count();

                                    if (TotalReadByte == BlockSize ||
                                        (TotalReadByte > BlockSize && TotalReadByte % BlockSize == 0))
                                    {
                                        BlockIndex++;
                                        BytesIndex = 0;
                                    }
                                }

                                //BinaryReader reader = new BinaryReader(fileStr);
                                //for (long index = 0; index < count; index++)
                                //{
                                //    if (index == count - 1)
                                //        buffer = reader.ReadBytes((int)remain);
                                //    else
                                //        buffer = reader.ReadBytes(1024);

                                //    DownloadManager.BlockCollection[BlockIndex].bytesCollection[BytesIndex++] = buffer;

                                //    TotalReadByte += buffer.Count();

                                //    if (TotalReadByte == BlockSize ||
                                //        (TotalReadByte > BlockSize && TotalReadByte % BlockSize == 0))
                                //    {
                                //        BlockIndex++;
                                //        BytesIndex = 0;
                                //    }
                                //}
                                //reader.Close();

                            }
                        }
                        catch (FileNotFoundException ioEx)
                        {
                            Console.WriteLine(ioEx.Message);
                        }

                        //CopyEnd

                        string path111 = @"C:\Users\jyou_estsecurity\Documents\repos\FileEditor_Toy\CommnunicationTest_Server\bin\Debug\Temp";
                        FileHelper.CreateDirectory(path111);

                        using (FileStream fileStr111 = new FileStream(Path.Combine(path111, DownloadManager.Fileinfo.Name), FileMode.Create, FileAccess.Write))
                        {
                            foreach (var Block in DownloadManager.BlockCollection)
                            {
                                foreach (var bytes in Block.bytesCollection)
                                {
                                    fileStr111.Write(bytes, 0, bytes.Count());
                                }
                            }
                        }

                        //FileStream fileStr111 = new FileStream(Path.Combine(path111, DownloadManager.Fileinfo.Name), FileMode.Create, FileAccess.Write);
                        //BinaryWriter writer = new BinaryWriter(fileStr111);
                        //foreach (var Block in DownloadManager.BlockCollection)
                        //{
                        //    foreach (var bytes in Block.bytesCollection)
                        //    {
                        //        writer.Write(bytes, 0, bytes.Count());
                        //    }
                        //}
                        //writer.Close();
                        //-----------


                        break;
                    }
                case "await":
                    {
                        Console.WriteLine("mainThread : 1");
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                        button1_Click();
                        break;
                    }

                case "base64":
                    {
                        var st = ToBase64Encode("aaa|aaa");
                        var st1 = ToBase64Encode("111|111");
                        var st2 = ToBase64Encode("222|222");
                        var st3 = ToBase64Encode("ES_Login_KEY");
                        break;
                    }

                case "DictionaryBinding":
                    {

                        ObservableDictionary.Add("김나영", new PersonB() { Age = 22222, Name = "2222" });


                        break;
                    }

                case "DictionaryBinding2":
                    {

                        ObservableDictionary["김유정"] = new PersonB() { Age = 33333, Name = "3333" };
                        break;
                    }
                case "observable":
                    {
                        Console.WriteLine($"Thread.CurrentThread : {Thread.CurrentThread.ManagedThreadId}");
                        GetUpdatePort(1).SubscribeOn(ThreadPoolScheduler.Instance).Subscribe(NextFunction, ErrorFunction , CompletedFunction );

                        Console.WriteLine($"observable");
                        
                        break;
                    }
                case "observable2":
                    {
                        Console.WriteLine($"Thread.CurrentThread : {Thread.CurrentThread.ManagedThreadId}");
                        GetUpdatePort(2).Subscribe(NextFunction, ErrorFunction, CompletedFunction);
                        Console.WriteLine($"observable1");
                        break;
                    }
                case "observable3":
                    {
                        break;
                    }


                default:
                    break;
                    
            }
        }

        private void NextFunction(int updatePort)
        {
            Console.WriteLine($"NextFunction");
            Console.WriteLine($"Thread.CurrentThread : {Thread.CurrentThread.ManagedThreadId}");
            ObservableText2 = "NextFunction";
            ObservableText = updatePort.ToString();
        }
        private void ErrorFunction(Exception e)
        {
            Console.WriteLine($"ErrorFunction");
            Console.WriteLine($"Thread.CurrentThread : {Thread.CurrentThread.ManagedThreadId}");
            ObservableText2 = "ErrorFunction";
            ObservableText1 = e.ToString();
        }
        private void CompletedFunction()
        {
            Console.WriteLine($"CompletedFunction");
            Console.WriteLine($"Thread.CurrentThread : {Thread.CurrentThread.ManagedThreadId}");
            ObservableText2 = "CompletedFunction";
        }

        public string ObservableText { get; set; } = "TT";
        public string ObservableText1 { get; set; } = "TT";
        public string ObservableText2 { get; set; } = "TT";

        private IObservable<int> GetUpdatePort(int i)
        {
            int updatePort = i;

            return Observable.Create<int>(s =>
            {
                var result = true;
                if (result)
                {
                    s.OnNext(updatePort);
                    s.OnCompleted();
                }
                else
                    s.OnError(new Exception("Asm Server Connect Error"));

                return Disposable.Empty;
            }).SubscribeOn(ThreadPoolScheduler.Instance);
            //});
            //}).SubscribeOn(TaskPoolScheduler.Default);
        }

        public Dictionary<string, string> Dictionary { get; set; } = new Dictionary<string, string>() { { "testKey", "testValue" } };
        public ObservableDictionary<string, PersonB> ObservableDictionary { get; set; } = new ObservableDictionary<string, PersonB>() { { "김유정", new PersonB { Age=1111,Name="1111"} } };

        public static string ToBase64Encode(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }

        public static string ToBase64Decode(string base64EncodedText)
        {
            if (String.IsNullOrEmpty(base64EncodedText))
            {
                return base64EncodedText;
            }

            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedText);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void button1_Click()
        {
            Console.WriteLine("button1_Click : 1");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Run();  //UI Thread에서 실행
            Console.WriteLine("button1_Click end");

        }

        private async void Run()
        {
            Console.WriteLine("Run : 1");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            int sum = await LongCalc2(5);

            Console.WriteLine("check point");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Run end");

        }

        private async Task<int> LongCalc2(int times)
        {
            //UI Thread에서 실행
            Console.WriteLine("LongCalc2 : 1");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            int result = 0;
            for (int i = 0; i < times; i++)
            {
                result += i;
                Console.WriteLine($"LongCalc2 : for {i}");
                await Task.Delay(1000);
            }
            Console.WriteLine("LongCalc2 for end : 1");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            var sum2 = await LongCalc3(5);
            var sum = await Task.Factory.StartNew(() => LongCalc4(5));
            var sum3 = await Task.Run(() => LongCalc5(5));

            Console.WriteLine("LongCalc2 end");

            return result;
        }

        private async Task<int> LongCalc3(int times)
        {
            Console.WriteLine("LongCalc3 : 1");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            //UI Thread에서 실행
            int result = 0;
            for (int i = 0; i < times; i++)
            {
                result += i;
                Console.WriteLine($"LongCalc3 : for {i}");
                await Task.Delay(1000);
            }

            Console.WriteLine("LongCalc3 end");

            return result;
        }

        private async Task<int> LongCalc4(int times)
        {
            Console.WriteLine("LongCalc4 : 3");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            //UI Thread에서 실행
            int result = 0;
            for (int i = 0; i < times; i++)
            {
                result += i;
                Console.WriteLine($"LongCalc4 : for {i}");
                await Task.Delay(1000);
            }

            Console.WriteLine("LongCalc4 end");

            return result;
        }
        private async Task<int> LongCalc5(int times)
        {
            Console.WriteLine("LongCalc5 : 3");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            //UI Thread에서 실행
            int result = 0;
            for (int i = 0; i < times; i++)
            {
                result += i;
                Console.WriteLine($"LongCalc5 : for {i}");
                await Task.Delay(1000);
            }

            Console.WriteLine("LongCalc5 end");

            return result;
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
    public class DownloadManager
    {
        public readonly int BlockSize = 1024 * 1024 * 1024;

        public FileInfo Fileinfo { get; set; }

        public ObservableCollection<Block> BlockCollection { get; set; } = new ObservableCollection<Block>();

        public long BlockCount
        {
            get
            {
                if (Fileinfo != null)
                {
                    if (Fileinfo.Length < BlockSize)
                        return 1;

                    if (Fileinfo.Length / BlockSize == 0)
                        return Fileinfo.Length / BlockSize;
                    else
                        return Fileinfo.Length / BlockSize + 1;
                }
                return -1;
            }
        }
    }

    public class Block
    {
        public int index;
        public string FileName;
        public string SaveFileName { get => $"{FileName}_{index}.tmp"; }

        public long Size;

        public readonly int ReadByteSize = 1024;

        public int totalReadByteSize
        {
            get
            {
                int Totalcount = 0;

                foreach (var bytes in bytesCollection)
                {
                    Totalcount += bytes.Count();
                }

                return Totalcount;
            }
        }

        public bool isFull { get => totalReadByteSize >= Size; }

        public ObservableCollection<byte[]> bytesCollection { get; set; } = new ObservableCollection<byte[]>();

        public Block(string _FileName, int _index, long _Size)
        {
            FileName = _FileName;
            index = _index;
            Size = _Size;
            InitbytesCollection();
        }

        private void InitbytesCollection()
        {
            for (int i = 0; i < bytesLastIndex + 1; i++)
            {
                bytesCollection.Add(new byte[] { });
            }
        }

        public long bytesLastIndex
        {
            get
            {
                if (Size % ReadByteSize == 0)
                    return Size / ReadByteSize - 1;
                else
                    return Size / ReadByteSize;

            }
        }
    }
}
