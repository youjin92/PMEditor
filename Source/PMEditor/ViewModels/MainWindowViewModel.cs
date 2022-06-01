using Common.Capture;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;
using System.Windows.Controls;
using Rectangle = System.Windows.Shapes.Rectangle;
using Image = System.Windows.Controls.Image;
using Common;
using System.IO;
using Tesseract;
using Common.Excel;
using Prism.Events;
using Common.PubSubEvents;
using System.Windows.Threading;
using static Common.CommonManager;
using PMEditor.Views;
using Prism.Regions;
using Common.OCR;
using Prism.Services.Dialogs;
using Common.IService;
using System.Threading.Tasks;

namespace PMEditor.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel(Grid grid , IEventAggregator eventAggregator, IRegionManager regionManager, IDialogService dialogService, ISolutionManager _SolutionManager)
        {
            rootgrid = grid;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _dialogService = dialogService;

            SolutionManager = _SolutionManager;

            _eventAggregator.GetEvent<CaptureAndContrastImageEvent>().Subscribe(SaveCaptureImageAndContrastImageFromEvent);
        }

        #region 프로퍼티
        public string Title { get; set; } = "PMEditor";
        public string FirstLineText { get; set; } = "암호 해독중입니다...";
        public string SecondLineText { get; set; } = "";
        public Visibility IsBorderVisible { get; set; } = Visibility.Collapsed;
        public Visibility IsProblemEndingBorderVisibility { get; set; } = Visibility.Collapsed;
        public HorizontalAlignment TextBlockHorizontal { get; set; } = HorizontalAlignment.Left;
        public VerticalAlignment TextBlockVertical { get; set; } = VerticalAlignment.Top;
        public string ResultText {
            get => _ResultText;
            set
            {
                _ResultText = value;
            } 
        } 
        public bool IsOcrThreadChecked { get; set; }
        public ISolutionManager SolutionManager { get; set; }
        #endregion

        #region 필드
        string _ResultText = "Error";
        Grid rootgrid;
        Point startPosition;
        Point endPosition;
        Rectangle currentVisibleRect;
        System.Windows.Rect OCRRect;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        public Thread OCRThread;
        InfoSubWinodow PropertyWindow = null;        
        #endregion

        #region 커멘드
        private DelegateCommand _CreateRectCommand;
        public DelegateCommand CreateRectCommand => _CreateRectCommand ?? (_CreateRectCommand = new DelegateCommand(ExecuteCreateRectCommand));
        void ExecuteCreateRectCommand()
        {
            if (currentVisibleRect != null)
            {
                ClearRectagle();
                currentVisibleRect = null;
            }

            IsBorderVisible = Visibility.Visible;
        }

        private DelegateCommand<object> _MouseLeftButtonDownCommand;
        public DelegateCommand<object> MouseLeftButtonDownCommand =>_MouseLeftButtonDownCommand ?? (_MouseLeftButtonDownCommand = new DelegateCommand<object>(ExecuteMouseLeftButtonDownCommand));
        void ExecuteMouseLeftButtonDownCommand(object parameter)
        {
            var e = parameter as MouseEventArgs;

            //마우스의 좌표를 저장한다.
            startPosition = e.GetPosition(rootgrid);
            //마우스가 Grid밖으로 나가도 위치를 알 수 있도록 마우스 이벤트를 캡처한다.
            rootgrid.CaptureMouse();

            if (IsBorderVisible == Visibility.Visible)
            {
                CreateRectangle();
            }
        }



        private DelegateCommand<object> _MouseLeftButtonUpCommand;
        public DelegateCommand<object> MouseLeftButtonUpCommand => _MouseLeftButtonUpCommand ?? (_MouseLeftButtonUpCommand = new DelegateCommand<object>(ExecuteMouseLeftButtonUpCommandName));
        void ExecuteMouseLeftButtonUpCommandName(object parameter)
        {
            var e = parameter as MouseEventArgs;

            //마우스의 좌표를 저장한다.
            endPosition = e.GetPosition(rootgrid);
            //마우스 캡춰를 제거한다.
            rootgrid.ReleaseMouseCapture();

            if (IsBorderVisible == Visibility.Visible)
            {
                SetRectangleProperty();
                CreateOCRRect(startPosition, endPosition);

                IsBorderVisible = Visibility.Collapsed;
            }
        }


        private DelegateCommand<object> _MouseMoveCommand;
        public DelegateCommand<object> MouseMoveCommand => _MouseMoveCommand ?? (_MouseMoveCommand = new DelegateCommand<object>(ExecuteMouseMoveCommandCommandName));
        void ExecuteMouseMoveCommandCommandName(object parameter)
        {
            var e = parameter as MouseEventArgs;

            //현재 이동한 마우스의 좌표를 얻어온다
            Point currnetPosition = e.GetPosition(rootgrid);

            //마우스 왼쪽 버튼이 눌려있으면
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                if (currentVisibleRect != null && IsBorderVisible == Visibility.Visible)
                {
                    //사각형이 나타날 기준점을 설정한다.
                    double left = startPosition.X;
                    double top = startPosition.Y;
                    //마우스의 위치에 따라 적절히 기준점을 변경한다.
                    if (startPosition.X > currnetPosition.X)
                    {
                        left = currnetPosition.X;
                    }
                    if (startPosition.Y > currnetPosition.Y)
                    {
                        top = currnetPosition.Y;
                    }
                    //사각형의 위치 기준점(Margin)을 설정한다
                    currentVisibleRect.Margin = new Thickness(left, top, 0, 0);
                    //사각형의 크기를 설정한다. 음수가 나올 수 없으므로 절대값을 취해준다.
                    currentVisibleRect.Width = Math.Abs(startPosition.X - currnetPosition.X);
                    currentVisibleRect.Height = Math.Abs(startPosition.Y - currnetPosition.Y);
                }
            }
        }

        private DelegateCommand<string> _MoveToUICommand;
        public DelegateCommand<string> MoveToUICommand => _MoveToUICommand ?? (_MoveToUICommand = new DelegateCommand<string>(ExecuteMoveToCommand));
        void ExecuteMoveToCommand(string direction)
        {
            switch (direction)
            {
                case "Left":
                    TextBlockHorizontal = HorizontalAlignment.Left;
                    break;
                case "Right":
                    TextBlockHorizontal = HorizontalAlignment.Right;
                    break;
                case "Up":
                    TextBlockVertical = VerticalAlignment.Top;
                    break;
                case "Down":
                    TextBlockVertical = VerticalAlignment.Bottom;
                    break;
                default:
                    break;
            }

        }

        private DelegateCommand<string> _PropertyViewVisibleCommand;
        public DelegateCommand<string> PropertyViewVisibleCommand =>_PropertyViewVisibleCommand ?? (_PropertyViewVisibleCommand = new DelegateCommand<string>(ExecutePropertyViewVisibleCommandName));
        void ExecutePropertyViewVisibleCommandName(string param)
        {
            DispatcherService.Invoke((System.Action)(() =>
            {
                switch (param)
                {
                    case "PropertyView":
                        {
                            if (PropertyWindow == null)
                            {
                                PropertyWindow = new InfoSubWinodow();
                                PropertyWindow.Closed += TempClosed;
                                PropertyWindow.Show();
                                break;
                            }
                            else
                            {
                                PropertyWindow.Focus();
                                break;
                            }
                        }
                    default:
                        break;
                }
            }));

            void TempClosed(object sender, EventArgs e)
            {
                PropertyWindow = null;
            }
        }

        private DelegateCommand<string> _OCRToggleCommand;
        public DelegateCommand<string> OCRToggleCommand => _OCRToggleCommand ?? (_OCRToggleCommand = new DelegateCommand<string>(ExecuteOCRToggleCommand));
        void ExecuteOCRToggleCommand(string parameter)
        {
            DispatcherService.Invoke((System.Action)(() =>
            {
                switch (parameter)
                {
                    case "Checked":
                        {
                            OCRManager.IsOcrRunning = true;
                            OCRThread = new Thread(() =>
                            {
                                while (true)
                                {
                                    SaveCaptureImageAndContrastImage();

                                    string ocredText = OCRManager.OCR($"{FileManager.ImageRootPath}\\contract.png");
                                    ResultText = ocredText;

                                    if (ResultText.Length <= 15)
                                    {
                                        DispatcherService.Invoke((System.Action)(() =>
                                        {
                                            _eventAggregator.GetEvent<SearchItemEvent>().Publish(new EventParam(ResultText));
                                        }));
                                    }
                                }
                            });
                            OCRThread.Start();
                            break;
                        }
                    case "UnChecked":
                        {
                            OCRManager.IsOcrRunning = false;
                            OCRThread.Abort();
                            break;
                        }
                    default:
                        break;
                }
            }));
        }

        private DelegateCommand _ClosedCommand;
        public DelegateCommand ClosedCommand => _ClosedCommand ?? (_ClosedCommand = new DelegateCommand(ExecuteClosedCommand));
        void ExecuteClosedCommand()
        {
            if (OCRThread != null && OCRThread.IsAlive)
                OCRThread.Abort();

            if (PropertyWindow != null)
                PropertyWindow.Close();
        }

        private DelegateCommand _TestCommand;
        public DelegateCommand TestCommand =>_TestCommand ?? (_TestCommand = new DelegateCommand(ExecuteTestCommand));
        void ExecuteTestCommand()
        {
            SaveCaptureImageAndContrastImage();

            string ocredText = OCRManager.OCR($"{FileManager.ImageRootPath}\\contract.png");
            ResultText = ocredText;


            if (ResultText.Contains("도전"))
            {
                IsProblemEndingBorderVisibility = Visibility.Visible;
                FirstAsync();
                SecondAsync();
            }
        }

        private async void FirstAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(2000);
                FirstLineText = "암호해독 완료";
                Thread.Sleep(2000);
            });
        }

        private async void SecondAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(5000);
                SecondLineText = "바이러스 제거";
                Thread.Sleep(3000);
            });
        }



        #endregion

        #region 사각형 (Rectangle / OCRRect 생성)
        private void CreateRectangle()
        {
            currentVisibleRect = new Rectangle();
            currentVisibleRect.Stroke = new SolidColorBrush(Colors.Orange);
            currentVisibleRect.StrokeThickness = 2;
            currentVisibleRect.Opacity = 0.7;
            //사각형을 그리는 동안은 테두리를 Dash 스타일로 설정한다.
            DoubleCollection dashSize = new DoubleCollection();
            dashSize.Add(1);
            dashSize.Add(1);
            currentVisibleRect.StrokeDashArray = dashSize;
            //사각형의 정렬 기준을 설정한다.
            currentVisibleRect.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            currentVisibleRect.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            //그리드에 추가한다.
            rootgrid.Children.Add(currentVisibleRect);
        }

        private void CreateOCRRect(Point startPoint, Point endPoint)
        {
            double MinX; double MaxX;
            double MinY; double MaxY;

            if (startPoint.X > endPoint.X)
            { MaxX = startPoint.X; MinX = endPoint.X; }
            else
            { MinX = startPoint.X; MaxX = endPoint.X; }

            if (startPoint.Y > endPoint.Y)
            { MaxY = startPoint.Y; MinY = endPoint.Y; }
            else
            { MinY = startPoint.Y; MaxY = endPoint.Y; }

            OCRRect = new System.Windows.Rect(new Point(MinX, MinY), new Point(MaxX,MaxY));
        }

        private void SetRectangleProperty()
        {
            //사각형의 투명도를 100% 로 설정
            currentVisibleRect.Opacity = 1;
            //사각형의 테두리를 선으로 지정
            currentVisibleRect.StrokeDashArray = new DoubleCollection(); ;
        }

        private void SaveCaptureImageAndContrastImageFromEvent()
        {
            if (!IsOcrThreadChecked)
            {
                SaveCaptureImageAndContrastImage();
            }
        }

        private void SaveCaptureImageAndContrastImage()
        {
            CapureManager.SaveCaptureImage((int)OCRRect.X, (int)OCRRect.Y, (int)OCRRect.Width, (int)OCRRect.Height, $"{FileManager.ImageRootPath}\\capture.png");
            CapureManager.CutAsBorder($"{FileManager.ImageRootPath}\\capture.png", OCRRect);

            CapureManager.SaveContrastImage();
        }

        private void ClearRectagle()
        {
            if (rootgrid.Children.Count > 0)
            {
                for (int i = 0; i < rootgrid.Children.Count; i++)
                {
                    if (rootgrid.Children[i].GetType() == typeof(Rectangle))
                    {
                        rootgrid.Children.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
