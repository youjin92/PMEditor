using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class FileManager
    {
        public static  string ImageRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image");
        public static  string ContractImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image");
        public static string DBPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");

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
    }
}
