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

        MySqlConnection connection;


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
                        connection = new MySqlConnection("Server=127.0.0.1;Database=member;Uid=root;Pwd=1q2w3e4r!!;");
                        break;
                    }

                case "AddItem":
                    {
                        string insertQuery = $"INSERT INTO member_table(name,age) VALUES('{Name}', {Age})";

                        connection.Open();
                        MySqlCommand command = new MySqlCommand(insertQuery, connection);

                        try//예외 처리
                        {
                            // 만약에 내가처리한 Mysql에 정상적으로 들어갔다면 메세지를 보여주라는 뜻이다
                            if (command.ExecuteNonQuery() == 1)
                            {
                                MessageBox.Show("정상적으로 갔다");
                            }
                            else
                            {
                                MessageBox.Show("비정상 이당");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }


                        connection.Close();
                        break;

                    }

                case "Write":
                    {
                        connection.Open();
                        string insertQuery = $"UPDATE member.member_table SET age={WriteAge} WHERE name='{WriteName}'";

                        MySqlCommand command = new MySqlCommand(insertQuery, connection);

                        try//예외 처리
                        {
                            // 만약에 내가처리한 Mysql에 정상적으로 들어갔다면 메세지를 보여주라는 뜻이다
                            if (command.ExecuteNonQuery() == 1)
                            {
                                MessageBox.Show("정상적으로 갔다");
                            }
                            else
                            {
                                MessageBox.Show("비정상 이당");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        connection.Close();

                        break;
                    }

                case "Read":
                    {
                        try
                        {
                            connection.Open();
                            string sql = $"SELECT name, age FROM member.member_table WHERE name='{ReadName}'";

                            MySqlCommand cmd = new MySqlCommand(sql, connection);
                            MySqlDataReader rdr = cmd.ExecuteReader();

                            while (rdr.Read())
                            {
                                Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                                ReadName = rdr[0].ToString();
                                ReadAge = Convert.ToInt32( rdr[1] );
                            }

                            rdr.Close();
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    }

                case "Delete":
                    {
                        connection.Open();
                        string insertQuery = $"DELETE FROM member.member_table WHERE name='{DeleteName}'";

                        MySqlCommand command = new MySqlCommand(insertQuery, connection);

                        try//예외 처리
                        {
                            // 만약에 내가처리한 Mysql에 정상적으로 들어갔다면 메세지를 보여주라는 뜻이다
                            if (command.ExecuteNonQuery() == 1)
                            {
                                MessageBox.Show("정상적으로 갔다");
                            }
                            else
                            {
                                MessageBox.Show("비정상 이당");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        connection.Close();
                        break;
                    }

                default:
                    break;

            }
        }

    }
}
