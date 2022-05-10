using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PMEditor.SampleModule.Views
{
    /// <summary>
    /// Interaction logic for ProblemView
    /// </summary>
    public partial class ProblemView : UserControl
    {
        public ProblemView()
        {
            InitializeComponent();
        }

        public ImageSource ImageSourceEX
        {
            get { return (ImageSource)GetValue(ImageSourceEXProperty); }
            set { SetValue(ImageSourceEXProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ImageSourceEX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceEXProperty = DependencyProperty.Register("ImageSourceEX", typeof(ImageSource),
            typeof(ProblemView), new FrameworkPropertyMetadata(new PropertyChangedCallback(ImageSourcePropertyChanged)));

        public static void ImageSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProblemView obj = d as ProblemView;

            obj.SetImageSource(e.NewValue as ImageSource);
        }

        private void SetImageSource(ImageSource imagesource)
        {
            image.Source = imagesource;
        }

        public string Button1Content
        {
            get { return (string)GetValue(Button1ContentProperty); }
            set { SetValue(Button1ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Button1Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Button1ContentProperty =
            DependencyProperty.Register("Button1Content", typeof(string), typeof(ProblemView),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(Button1ContentPropertyChanged)));

        private static void Button1ContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProblemView obj = d as ProblemView;
            obj.Button1.Content = e.NewValue.ToString();
        }

        public string Button2Content
        {
            get { return (string)GetValue(Button2ContentProperty); }
            set { SetValue(Button2ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Button1Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Button2ContentProperty =
            DependencyProperty.Register("Button2Content", typeof(string), typeof(ProblemView),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(Button2ContentPropertyChanged)));

        private static void Button2ContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProblemView obj = d as ProblemView;
            obj.Button2.Content = e.NewValue.ToString();
        }

        public string Answer { get; set; } = "test";
        public int Order { get; set; } = 1;

    }
}
