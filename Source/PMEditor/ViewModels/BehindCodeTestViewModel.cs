using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

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
            string path = @"C:\Folder1\Folder2\Folder3\Folder4\";
            string folderName = "Test.txt";
            string newPath = "";
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

                    break;
                default:
                    break;
            }


        }
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




    }
}
