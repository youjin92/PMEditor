using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class FileManager
    {
        public static  string ImageRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");

        public static string DBPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");
        public static string DownloadFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download");

        public static  string TessadataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tessdata");
        public static string ResourceFielPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource");

        public static string ImagePath;

        public static bool IsFileExist(string strFile)
        {
            FileInfo fileInfo = new FileInfo(strFile);
            return fileInfo.Exists;
        }

        public static bool RemoveFile(string strFile)
        {
            try
            {
                System.IO.File.Delete(strFile);
                return true;
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool CheckInstalledApplications(string programName)
        {
            bool isInstalled = false;

            foreach (string item in Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall").GetSubKeyNames())
            {
                object itemProgramName = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + item).GetValue("DisplayName");

                Console.WriteLine(itemProgramName);

                if (string.Equals(itemProgramName, programName))
                {
                    Console.WriteLine("Install status: INSTALLED");
                    isInstalled = true;
                    break;
                }
            }
            return isInstalled;
        }

        public static void InstallApplication(string programName, bool IsWaitforExit = false)
        {
            Process ExternalProcess = new Process();
            ExternalProcess.StartInfo.FileName = DownloadFilePath + "\\" + programName; // @"C:\Users\Vitor\ConsoleApplication1.exe";
            //ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

            //실행시킬 프로그램윈도우 크기 
            ExternalProcess.Start();//process 시작
            if(IsWaitforExit)
                ExternalProcess.WaitForExit(); //외부 process 시작되면 실행중인 c# 일시중지
        }


    }
}
