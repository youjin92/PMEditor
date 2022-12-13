using Common.Model;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static Common.ShellApiWrapper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Net.Http;
using System.Net;

namespace PMEditor.ViewModels
{
    public class BehindCodeTest4ViewModel : BindableBase
    {
        public string Text { get; set; } = "Text";
        public string Text2 { get; set; } = "Text2";
        public string DPText { get; set; } = "bbbaaabbb";

        public ImageSource Source { get; set; }

        private DelegateCommand<string> _TestCommand;
        public DelegateCommand<string> TestCommand => _TestCommand ?? (_TestCommand = new DelegateCommand<string>(ExecuteTestCommand));
        void ExecuteTestCommand(string param)
        {

            switch (param)
            {
                case "Convert":
                    {

                        break;
                    }

                case "Input":
                    {
                        DPText = "start";
                        break;
                    }
                case "Test":
                    {
                        System.Windows.Controls.Button btn = null;
                        System.Windows.Controls.Button btn1 = new System.Windows.Controls.Button();

                        btn1.Content = "bbb";

                        bool a = btn1?.Content?.ToString() == "bbb";
                        bool b = btn?.Content?.ToString() == "bbb";
                        bool c = null == "nnn";

                        break;
                    }
                case "EventWaitHandle":
                    {
                        Task.Run(() => CheckLogin());
                        break;
                    }
                case "ForeachCallbyreference":
                    {
                        List<PersonB> intarray = new List<PersonB> { new PersonB { Name = "aaa", Age = 19 }, new PersonB { Name = "bbb", Age = 19 } };

                        foreach (PersonB item in intarray)
                        {
                            if (item.Age == 19)
                                item.Age = 20;
                        }

                        List<int> intarray2 = new List<int> { 1,2,3,4,5,6,7,8,9 };

                        foreach (int item in intarray2)
                        {

                        }

                        break;
                    }
                case "httpRequest":
                    {
                        //string url = @"https://www.op.gg/summoners/kr/신림동김용문";
                        string url = @"https://kr.whatismymmr.com/신림동김용문";
                        //string url = "https://naver.com/get";
                        //test url
                        string responseText = string.Empty;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "GET";
                        request.Timeout = 30 * 1000;
                        // 30 * 1000ms
                        request.Headers.Add("Authorization", "SEG=+8dq");
                        // 헤더 추가

                        using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                        {
                            HttpStatusCode status = resp.StatusCode;
                            // response 매시지 중 StatusCode를 가져온다.

                            Console.WriteLine(status);
                            // 정상이면 "OK"

                            Stream respStream = resp.GetResponseStream();
                            // Response Data 내용은 GetResponseStream 메서드로부터 얻어낸 스트림을 읽어 가져올 수 있음
                            using (StreamReader sr = new StreamReader(respStream))
                            {
                                responseText = sr.ReadToEnd();
                            }
                        }

                        Console.WriteLine(responseText);
                        break;
                    }

                case "LinqTest":
                    {
                        ObservableCollection<int> a = new ObservableCollection<int>() { 1, 2, 3, 4, 5, 6, 7 };

                        int[] d = new int[] { 22, 3, 4, 5, 6, 78, };

                        var c = a.Where(e => e > 3).FirstOrDefault();

                        var f = d.Where(e => e > 3).FirstOrDefault();

                        if (a.Where(e => e > 3).Any())
                        {
                            c = 8;
                        }

                        if (a.Where(e => e > 3).Any(e => e > 8))
                        {
                            c = 8;
                        }

                        break;
                    }

                case "DumpTest":
                    {
                        int a = 0;
                        double b = 10 / a;
                        break;
                    }
                default:
                    break;

            }
        }

        private EventWaitHandle loginCheckEvent = null;
        private void CheckLogin()
        {
            EventWaitHandle.TryOpenExisting("Test_EventWaitHandle", out loginCheckEvent);
            if (loginCheckEvent != null)
            {
                while (true)
                {
                    Thread.Sleep(500);

                    if (loginCheckEvent.IsSignaled())
                    {
                        DPText = "Signaled";
                        break;
                    }
                }
            }
        }

    }
    public static class ExtEventWaitHandle
    {
        public static bool IsSignaled(this EventWaitHandle eventObj)
        {
            return (eventObj?.WaitOne(0) == true) ? true : false;
        }
    }
}
