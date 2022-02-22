using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DB
{
    public static class DBManager
    {
        public static string DBPath = FileManager.DBPath + @"\Poketmon.db";
        public static SQLiteConnection ConnectedOBJ;

        public static void CreatDBFile()
        {
            try
            {
                // sqlite.db가 해당 경로 폴더 안에 있는지 체크
                if (!System.IO.File.Exists(DBPath))
                {
                    SQLiteConnection.CreateFile(DBPath);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        public static void Connect()
        {
            ConnectedOBJ = new SQLiteConnection($"Data Source={DBPath};Version=3;");

            ConnectedOBJ.Open();
        }

        /// <summary>
        /// string sql = "create table members (name varchar(20), age int)";
        /// </summary>                    
        public static void CreateTable(string sql)
        {
            //string sql2 = "create table members (name varchar(20), age int)";
            SQLiteCommand command = new SQLiteCommand(sql, ConnectedOBJ);
            int result = command.ExecuteNonQuery();
        }

        /// <summary>
        /// String sql = "insert into members (name, age) values ('김도현', 6)";
        /// </summary> 
        public static void InsertRow(string sql)
        {
            //String sql = "insert into members (name, age) values ('김도현', 6)";
            SQLiteCommand command = new SQLiteCommand(sql, ConnectedOBJ);
            int result = command.ExecuteNonQuery();
        }

        /// <summary>
        /// String sql = "select * from members";
        /// </summary> 
        public static void ReadRow(string sql, Action<SQLiteDataReader> action)
        {
            //String sql = "select * from members";
            SQLiteCommand cmd = new SQLiteCommand(sql, ConnectedOBJ);
            SQLiteDataReader rdr;
            try
            {
                rdr = cmd.ExecuteReader();
                action(rdr);
                rdr.Close();
            }
            catch
            {
                rdr = null;
            }
            finally
            {
            }
        }

        public static void Close()
        {
            ConnectedOBJ.Close();
        }
    }
}
