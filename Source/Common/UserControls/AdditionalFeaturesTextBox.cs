using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Common.UserControls
{
    public class  AdditionalFeaturesTextBox : System.Windows.Controls.UserControl
    {
        System.Windows.Controls.StackPanel mainStackPanel = new System.Windows.Controls.StackPanel();

        public void SetmainStackPanel(string InputText)
        {
            Console.WriteLine("SetmainStackPanel");
            mainStackPanel.Children.Clear();

            if (!IsUsingOption)
            {
                //Text 
                string TempText = InputText;

                if (ContainsCount(InputText, "_%B") > 0)
                {
                    TempText = TempText.Replace("_%B", "");
                }

                if (ContainsCount(InputText, "B%_") > 0)
                {
                    TempText = TempText.Replace("B%_", "");
                }

                mainStackPanel.Children.Add(new System.Windows.Controls.TextBlock() { Text = TempText });
            }
            else
            {
                //기능 사용
                int _BCount = ContainsCount(InputText, "_%B");
                int B_Count = ContainsCount(InputText, "B%_");

                if (_BCount != B_Count)
                {
                    mainStackPanel.Children.Add(new System.Windows.Controls.TextBlock() { Text = $"Error : {InputText}" });
                }
                string Temp = InputText;

                int UsingCount = 0;
                while (UsingCount != B_Count)
                {
                    int _Bindex = Temp.IndexOf("_%B");

                    if (_Bindex > 0)
                    {
                        System.Windows.Controls.TextBlock textBlock = new System.Windows.Controls.TextBlock() ;
                        textBlock.Text = Left(Temp, _Bindex);

                        mainStackPanel.Children.Add(textBlock);
                    }

                    Temp = Temp.Substring(_Bindex + 3);

                    int B_index = Temp.IndexOf("B%_");

                    System.Windows.Controls.TextBlock textBlock2 = new System.Windows.Controls.TextBlock();
                    textBlock2.FontWeight = FontWeights.Bold;
                    textBlock2.Foreground = Brushes.Red;
                    textBlock2.Text = Left(Temp, B_index);

                    mainStackPanel.Children.Add(textBlock2);
                    Temp = Temp.Substring(B_index + 3);
                    UsingCount++;
                }

                if (Temp.Length > 0)
                {
                    mainStackPanel.Children.Add( new System.Windows.Controls.TextBlock() { Text = Temp});
                }
            }
        }



        private int ContainsCount(string InputText, string SearchWord)
        {
            int Count = 0;
            string Temp = InputText;

            while (Temp.Contains(SearchWord))
            {
                Count++;

                int Index = Temp.IndexOf(SearchWord) + SearchWord.Length;

                Temp = Right(Temp, Temp.Length - Index);

            }

            return Count;   
        }

        public string Right(string str, int Length)
        {
            if (str.Length < Length)
                Length = str.Length;

            return str.Substring(str.Length - Length, Length);
        }

        public string Left(string str, int Length)
        {
            if (str.Length < Length)
                Length = str.Length;

            return str.Substring(0, Length);
        }

        public AdditionalFeaturesTextBox()
        {
            this.Content = mainStackPanel;
            mainStackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AdditionalFeaturesTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPropertyChanged)));

        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdditionalFeaturesTextBox obj = d as AdditionalFeaturesTextBox;

            obj.SetmainStackPanel(e.NewValue.ToString()) ;

        }

        public bool IsUsingOption
        {
            get { return (bool)GetValue(IsUsingOptionProperty); }
            set { SetValue(IsUsingOptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUsingOption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUsingOptionProperty =
            DependencyProperty.Register("IsUsingOption", typeof(bool), typeof(AdditionalFeaturesTextBox), new PropertyMetadata(false));
    }
}
