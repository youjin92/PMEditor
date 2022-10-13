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

namespace PMEditor.ViewModels
{
    public class BehindCodeTest2ViewModel : BindableBase
    {
        public string Text { get; set; } = "TEST";
        public BehindCodeTest2ViewModel()
        {

        }

        public ImageSource Source1 { get; set; } 
        public ImageSource Source2 { get; set; } 
        public ImageSource Source3 { get; set; } 
        public ImageSource Source4 { get; set; } 
        public ImageSource Source5 { get; set; } 
        public ImageSource Source6 { get; set; } 
        public ImageSource Source7 { get; set; } 

        private DelegateCommand<string> _TestCommand;
        public DelegateCommand<string> TestCommand => _TestCommand ?? (_TestCommand = new DelegateCommand<string>(ExecuteTestCommand));
        void ExecuteTestCommand(string param)
        {
            switch (param)
            {
                case "ArrivalsMonitor":
                    {
                        BaggageHandler provider = new BaggageHandler();
                        ArrivalsMonitor observer1 = new ArrivalsMonitor("BaggageClaimMonitor1");
                        ArrivalsMonitor observer2 = new ArrivalsMonitor("SecurityExit");

                        provider.BaggageStatus(712, "Detroit", 3);
                        observer1.Subscribe(provider);
                        provider.BaggageStatus(712, "Kalamazoo", 3);
                        provider.BaggageStatus(400, "New York-Kennedy", 1);
                        provider.BaggageStatus(712, "Detroit", 3);
                        observer2.Subscribe(provider);
                        provider.BaggageStatus(511, "San Francisco", 2);
                        provider.BaggageStatus(712);
                        observer2.Unsubscribe();
                        provider.BaggageStatus(400);
                        provider.LastBaggageClaimed();
                        break;

                    }

                case "Test":
                    {
                        UInt64 A = 0;
                        UInt64 B = 0;
                        UInt64 C = 0;

                        for (UInt64 i = 0; i < UInt64.MaxValue; i++)
                        {
                            A = i;
                            for (UInt64 j = 0; j < UInt64.MaxValue; j++)
                            {
                                B = j;
                                for (UInt64 k = 0; k < UInt64.MaxValue; k++)
                                {
                                    C = k;
                                    UInt64 tmp = A / (B + C) + B / (C + A ) + C /(B+A);

                                    if (tmp == 4)
                                        break;
                                }
                            }
                        }

                        Console.WriteLine($"A : {A} ");
                        Console.WriteLine($"B : {B} ");
                        Console.WriteLine($"C : {C} ");
                        break;
                    }

                case "Test2":
                    {
                        bool a = false;
                        bool b = false;
                        bool c = true;

                        var d = a && b || c;
                        var e = a && (b || c);

                        break;
                    }
                case "Test3":
                    {
                        ObjectCache cache = MemoryCache.Default;
                        CacheItemPolicy policy = new CacheItemPolicy();

                        List<string> filePaths = new List<string>();
                        filePaths.Add("c:\\새 폴더\\새로운 2.txt");
                        filePaths.Add("c:\\새 폴더\\새로운 3.txt");

                        policy.ChangeMonitors.Add(new
                        HostFileChangeMonitor(filePaths));

                        // Fetch the file contents.  
                        string fileContents = File.ReadAllText("c:\\새 폴더\\새로운 2.txt");
                        //string fileContent3s = File.ReadAllText("c:\\새 폴더\\새로운 3.txt");

                        cache.Set("filecontents", fileContents, policy);
                        //cache.Set("filecontent3s", fileContent3s, policy);

                        break;
                    }
                case "Test4":
                    {
                        ObjectCache cache = MemoryCache.Default;
                        string fileContents = cache["filecontents"] as string;

                        if (fileContents == null)
                        {
                            CacheItemPolicy policy = new CacheItemPolicy();
                            policy.AbsoluteExpiration =
                                DateTimeOffset.Now.AddSeconds(10.0);


                            List<string> filePaths = new List<string>();
                            filePaths.Add("c:\\새 폴더\\새로운 2.txt");
                            filePaths.Add("c:\\새 폴더\\새로운 3.txt");

                            policy.ChangeMonitors.Add(new
                                HostFileChangeMonitor(filePaths));

                            // Fetch the file contents.
                            fileContents = File.ReadAllText("c:\\새 폴더\\새로운 2.txt");

                            cache.Set("filecontents", fileContents, policy);
                        }

                        TestText = cache["filecontents"] as string; ;
                        TestText2 = cache["filecontent3s"] as string; ;
                        break;
                    }

                case "Test5":
                    {
                        var Today = DateTime.Now;
                        var Add13hour = Today.AddHours(13);

                        var test = Add13hour - Today;

                        var diff = test.Days;
                        var diff2 = Add13hour.Day - Today.Day;
                        

                        break;
                    }

                case "Test6":
                    {
                        /*
                          버전이 2자리로 변경되었으나 기존 버전 업데이트 에러를 해결하기 위해 
                          AsmConsoleVersion.xml 내용은 기존과 동일한 자리수를 유지

                          left : 현재버전 2자리
                          right : xml 내 currentVersion 4자리

                          앞에 2자리는 스킵
                        */
                        int ret = -1;

                        string left = "61.1201";
                        string right = "5.1.0.6002";

                        var leftVersion = left.Split('.');
                        var rightVersion = right.Split('.');

                        for (int i = 0; i < 2; i++)
                        {
                            int comp = CompareSector(leftVersion[leftVersion.Length - 2 + i], rightVersion[rightVersion.Length - 2 + i]);
                            if (comp != 0)
                                ret = comp;
                        }

                        Console.WriteLine(ret);

                        GetIconFromImage("");

                        break;
                    }

                default:
                    break;

            }
        }

        public Icon GetIconFromImage(string path)
        {
            var uri = new Uri("pack://application:,,,/Images/capture2.png");
            var bitmap_image = new BitmapImage(uri);

            var bitmap = BitmapImage2Bitmap(bitmap_image);

            return (Icon)Icon.FromHandle(bitmap.GetHicon()).Clone();
        }


        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public string TestText { get; set; } = "test";
        public string TestText2 { get; set; } = "test2";

        private int CompareSector(string left, string right)
        {
            if (int.TryParse(left, out int leftI) && int.TryParse(right, out int rightI))
            {
                return leftI.CompareTo(rightI);
            }
            else
            {
                if (left.Equals(right))
                    return 0;
                else
                    return -1;
            }
        }
    }

    public class BaggageInfo
    {
        private int flightNo;
        private string origin;
        private int location;

        internal BaggageInfo(int flight, string from, int carousel)
        {
            this.flightNo = flight;
            this.origin = from;
            this.location = carousel;
        }

        public int FlightNumber
        {
            get { return this.flightNo; }
        }

        public string From
        {
            get { return this.origin; }
        }

        public int Carousel
        {
            get { return this.location; }
        }
    }

    public class BaggageHandler : IObservable<BaggageInfo>
    {
        private List<IObserver<BaggageInfo>> observers;
        private List<BaggageInfo> flights;

        public BaggageHandler()
        {
            observers = new List<IObserver<BaggageInfo>>();
            flights = new List<BaggageInfo>();
        }

        public IDisposable Subscribe(IObserver<BaggageInfo> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                // Provide observer with existing data.
                foreach (var item in flights)
                    observer.OnNext(item);
            }
            return new Unsubscriber<BaggageInfo>(observers, observer);
        }

        // Called to indicate all baggage is now unloaded.
        public void BaggageStatus(int flightNo)
        {
            BaggageStatus(flightNo, String.Empty, 0);
        }

        public void BaggageStatus(int flightNo, string from, int carousel)
        {
            var info = new BaggageInfo(flightNo, from, carousel);

            // Carousel is assigned, so add new info object to list.
            if (carousel > 0 && !flights.Contains(info))
            {
                flights.Add(info);
                foreach (var observer in observers)
                    observer.OnNext(info);
            }
            else if (carousel == 0)
            {
                // Baggage claim for flight is done
                var flightsToRemove = new List<BaggageInfo>();
                foreach (var flight in flights)
                {
                    if (info.FlightNumber == flight.FlightNumber)
                    {
                        flightsToRemove.Add(flight);
                        foreach (var observer in observers)
                            observer.OnNext(info);
                    }
                }
                foreach (var flightToRemove in flightsToRemove)
                    flights.Remove(flightToRemove);

                flightsToRemove.Clear();
            }
        }

        public void LastBaggageClaimed()
        {
            foreach (var observer in observers)
                observer.OnCompleted();

            observers.Clear();
        }
    }

    internal class Unsubscriber<BaggageInfo> : IDisposable
    {
        private List<IObserver<BaggageInfo>> _observers;
        private IObserver<BaggageInfo> _observer;

        internal Unsubscriber(List<IObserver<BaggageInfo>> observers, IObserver<BaggageInfo> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

    public class ArrivalsMonitor : IObserver<BaggageInfo>
    {
        private string name;
        private List<string> flightInfos = new List<string>();
        private IDisposable cancellation;
        private string fmt = "{0,-20} {1,5}  {2, 3}";

        public ArrivalsMonitor(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("The observer must be assigned a name.");

            this.name = name;
        }

        public virtual void Subscribe(BaggageHandler provider)
        {
            cancellation = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            cancellation.Dispose();
            flightInfos.Clear();
        }

        public virtual void OnCompleted()
        {
            flightInfos.Clear();
        }

        // No implementation needed: Method is not called by the BaggageHandler class.
        public virtual void OnError(Exception e)
        {
            // No implementation.
        }

        // Update information.
        public virtual void OnNext(BaggageInfo info)
        {
            bool updated = false;

            // Flight has unloaded its baggage; remove from the monitor.
            if (info.Carousel == 0)
            {
                var flightsToRemove = new List<string>();
                string flightNo = String.Format("{0,5}", info.FlightNumber);

                foreach (var flightInfo in flightInfos)
                {
                    if (flightInfo.Substring(21, 5).Equals(flightNo))
                    {
                        flightsToRemove.Add(flightInfo);
                        updated = true;
                    }
                }
                foreach (var flightToRemove in flightsToRemove)
                    flightInfos.Remove(flightToRemove);

                flightsToRemove.Clear();
            }
            else
            {
                // Add flight if it does not exist in the collection.
                string flightInfo = String.Format(fmt, info.From, info.FlightNumber, info.Carousel);
                if (!flightInfos.Contains(flightInfo))
                {
                    flightInfos.Add(flightInfo);
                    updated = true;
                }
            }
            if (updated)
            {
                flightInfos.Sort();
                Console.WriteLine("Arrivals information from {0}", this.name);
                foreach (var flightInfo in flightInfos)
                    Console.WriteLine(flightInfo);

                Console.WriteLine();
            }
        }
    }
}
