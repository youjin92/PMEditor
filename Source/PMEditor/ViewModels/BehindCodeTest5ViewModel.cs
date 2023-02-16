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
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace PMEditor.ViewModels
{
    public class BehindCodeTest5ViewModel : BindableBase
    {
        public string Text { get; set; } = "TEST4";
        public string Name { get; set; } 
        public int Age { get; set; }
        public string WriteName { get; set; }
        public int WriteAge { get; set; }
        public string ReadName { get; set; }
        public int ReadAge { get; set; }
        public string DeleteName { get; set; }

        DBManager2 DBManager = new DBManager2();

        List<Player> playerlist = new List<Player>();


        public BehindCodeTest5ViewModel()
        {
        }
        private DelegateCommand<string> _TestCommand;
        public DelegateCommand<string> TestCommand => _TestCommand ?? (_TestCommand = new DelegateCommand<string>(ExecuteTestCommand));
        void ExecuteTestCommand(string param)
        {

            switch (param)
            {
                case "MySqlConnection":
                    {
                        DBManager.Connect();
                        break;
                    }

                case "AddItem":
                    {
                        DBManager.Insert(Name, Age);
                        break;

                    }

                case "Write":
                    {
                        DBManager.WriteMMR(WriteName, WriteAge);
                        break;
                    }

                case "Read":
                    {
                        playerlist.Clear();
                        DBManager.ReadDB((name, mmr) => {
                            playerlist.Add(new Player { Name = name, MMR = mmr });
                        });

                        break;
                    }

                case "Delete":
                    {
                      
                        break;
                    }

                default:
                    break;

            }
        }

    }

    public class DBManager2
    {
        public readonly string DBServerIP = "192.168.75.196";
        public readonly string Database = "member";
        public readonly string id = "root";
        public readonly string Pwd = "1q2w3e4r!!";

        MySqlConnection connection;

        public void Connect()
        {
            connection = new MySqlConnection($"Server={DBServerIP};Database={Database};Uid={id};Pwd={Pwd};");
        }

        public bool ExcuteCommand(string insertQuery, string name, int mmr)
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(insertQuery, connection);

            bool ret = false;

            try
            {
                if (command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine($"{new StackFrame(1, true).GetMethod().Name} Success => name : {name}, mmr : {mmr}");
                    ret = true;
                }
                else
                {
                    Console.WriteLine($"{new StackFrame(1, true).GetMethod().Name} Error => name : {name}, mmr : {mmr}"); 
                    ret = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                ret = false;
            }

            connection.Close();
            return ret;
        }


        public void Insert(string name, int mmr)
        {
            string insertQuery = $"INSERT INTO member_table(name,mmr) VALUES('{name}', {mmr})";

            ExcuteCommand(insertQuery, name, mmr);
        }

        public void WriteMMR(string name, int mmr)
        {
            string insertQuery = $"UPDATE member.member_table SET mmr={mmr} WHERE name='{name}'";

            ExcuteCommand(insertQuery, name, mmr);
        }

        public void ReadDB(Action<string, int> action)
        {
            try
            {
                connection.Open();
                string sql = $"SELECT name, mmr FROM member.member_table";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    action(rdr[0].ToString(), Convert.ToInt32(rdr[1]));
                }

                rdr.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public class Player : BindableBase
    {
        public string Name { get; set; } = "Name";
        public int MMR { get; set; } = 0;
    }
}
