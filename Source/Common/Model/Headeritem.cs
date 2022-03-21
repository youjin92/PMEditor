using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Brushes = System.Windows.Media.Brushes;

namespace Common.Model
{
    public class Headeritem : UserControl
    {
        // 아이템 타입을 나타냅니다.
        public enum itemtypes
        {
            Number,
            Text,
            Resource
        }

        class NormalTextbox : TextBox
        {
            public NormalTextbox()
            {
                this.BorderBrush = null;
                this.BorderThickness = new Thickness(0);

                AddHandler(PreviewMouseLeftButtonDownEvent,
                    new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
                AddHandler(GotKeyboardFocusEvent,
                  new RoutedEventHandler(SelectAllText), true);
                AddHandler(MouseDoubleClickEvent,
                  new RoutedEventHandler(SelectAllText), true);
            }

            private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
            {
                // Find the TextBox
                DependencyObject parent = e.OriginalSource as UIElement;
                while (parent != null && !(parent is TextBox))
                    parent = VisualTreeHelper.GetParent(parent);

                if (parent != null)
                {
                    var textBox = (TextBox)parent;
                    if (!textBox.IsKeyboardFocusWithin)
                    {
                        // If the text box is not yet focussed, give it the focus and
                        // stop further processing of this click event.
                        textBox.Focus();
                        e.Handled = true;
                    }
                }
            }

            private static void SelectAllText(object sender, RoutedEventArgs e)
            {
                var textBox = e.OriginalSource as TextBox;
                if (textBox != null)
                    textBox.SelectAll();
            }




        }

        class NumberTextbox : NormalTextbox
        {
            public NumberTextbox()
            {

            }

            protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
            {
                e.Handled = !AreAllValidNumericChars(e.Text);
                base.OnPreviewTextInput(e);
            }

            bool AreAllValidNumericChars(string str)
            {
                bool ret = true;

                if (str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign)
                {
                    if (this.Text.Contains(str))
                    {
                        if (this.SelectionStart != 0)
                            return false;
                    }
                    else
                    {
                        if (this.Text.Length != 0)
                        {
                            if (this.SelectionStart != 0)
                                return false;
                        }
                    }
                }

                if (
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentGroupSeparator |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentSymbol |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.PerMilleSymbol |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol |
                    str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign)
                    return ret;

                int l = str.Length;

                if (System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign != str)
                {
                    for (int i = 0; i < l; i++)
                    {
                        char ch = str[i];
                        ret &= Char.IsDigit(ch);
                    }
                }

                return ret;
            }
        }


        /// <summary>
        /// 헤더를 생성 합니다.
        /// </summary>
        /// <param name="HeaderName"></param>
        /// <param name="isLast"></param>
        /// <param name="ItemType"></param>
        /// <returns></returns>
        private Border CreateBorder(string HeaderName, bool isLast, itemtypes ItemType)
        {
            Border headerBorder = new Border();

            // 테두리 정의
            headerBorder.BorderBrush = Brushes.Black;
            headerBorder.Background = Brushes.LightGray;

            if (isLast)
            {
                if (ItemType != itemtypes.Resource)
                    headerBorder.BorderThickness = new Thickness(1, 1, 1, 0);
                else
                    headerBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            }
            else
            {
                if (ItemType != itemtypes.Resource)
                    headerBorder.BorderThickness = new Thickness(1, 1, 0, 0);
                else
                    headerBorder.BorderThickness = new Thickness(1, 1, 0, 1);
            }

            // 헤더 이름
            TextBlock txtname = new TextBlock();
            txtname.FontSize = 11;
            txtname.Text = HeaderName;
            txtname.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            txtname.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txtname.TextAlignment = TextAlignment.Center;
            headerBorder.Child = txtname;
            Grid.SetRow(headerBorder, 0);

            return headerBorder;
        }

        // 헤더 전용으로 사용 됩니다.
        public Headeritem(string m_HeaderName, string m_Text, bool isLast = false, itemtypes ItemType = itemtypes.Number)
        {
            this.Width = 100;
            this.Height = 60;

            // 렌더링중에 겹치는 현상이 발생하여 사용함.
            this.SnapsToDevicePixels = true;

            // 그리드 정의
            Grid main = new Grid();
            main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25, GridUnitType.Pixel) });
            main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            // 헤더 정의
            main.Children.Add(CreateBorder(m_HeaderName, isLast, ItemType));

            // 숫자 전용 텍스트 박스 정의
            NumberTextbox txtinput = new NumberTextbox();
            txtinput.BorderBrush = Brushes.Black;

            if (isLast)
                txtinput.BorderThickness = new Thickness(1, 1, 1, 1);
            else
                txtinput.BorderThickness = new Thickness(1, 1, 0, 1);

            _Text = txtinput.Text = m_Text;
            txtinput.TextAlignment = TextAlignment.Center;
            txtinput.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            txtinput.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            txtinput.Padding = new Thickness(5, 1, 1, 1);
            txtinput.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            txtinput.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            txtinput.TextChanged += txtinput_TextChanged;

            Grid.SetRow(txtinput, 1);
            main.Children.Add(txtinput);

            this.Content = main;
        }

        public Headeritem()
        {
            itemtypes ItemType = itemtypes.Text;
            bool isLast = false;
            string HeaderName = "번호";
            this.Width = 100;
            this.Height = 60;

            //IsEnabled 변경에 따른 이벤트 추가
            IsEnabledChanged += ExcuteEnabledChanged;

            // 렌더링중에 겹치는 현상이 발생하여 사용함.
            this.SnapsToDevicePixels = true;

            Grid main = new Grid();
            main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25, GridUnitType.Pixel) });
            main.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            // 헤더 정의
            main.Children.Add(CreateBorder(HeaderName, isLast, ItemType));


            switch (ItemType)
            {
                case itemtypes.Text:
                    {
                        NormalTextbox txtinput = new NormalTextbox();
                        txtinput.BorderBrush = Brushes.Black;

                        if (isLast)
                            txtinput.BorderThickness = new Thickness(1, 1, 1, 1);
                        else
                            txtinput.BorderThickness = new Thickness(1, 1, 0, 1);

                        _Text = txtinput.Text = "txtinput.Text";

                        txtinput.TextAlignment = TextAlignment.Center;
                        txtinput.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                        txtinput.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                        txtinput.Padding = new Thickness(5, 1, 1, 1);
                        txtinput.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                        txtinput.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        txtinput.TextChanged += txtinput_TextChanged;

                        Grid.SetRow(txtinput, 1);
                        main.Children.Add(txtinput);
                        break;
                    }
                case itemtypes.Resource:
                    {
                        Button btn = new Button();
                        btn.BorderBrush = Brushes.Black;

                        if (isLast)
                            btn.BorderThickness = new Thickness(1, 1, 1, 1);
                        else
                            btn.BorderThickness = new Thickness(1, 1, 0, 1);

                        btn.Content = "txtinput.Text";

                        btn.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                        btn.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                        btn.Margin = new Thickness(1, 1, 1, 1);
                        btn.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                        btn.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        btn.Click += btn_Click;
                        Grid.SetRow(btn, 1);
                        main.Children.Add(btn);
                        break;
                    }
                default:
                    break;
            }


            this.Content = main;
        }

        private void ExcuteEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserControl obj = sender as UserControl;
            if (obj.IsEnabled)
                obj.Background = Brushes.Transparent;
            else
                obj.Background = Brushes.DarkGray;

        }

        // 문자열 리소스 일 경우 이벤트 발생
        void btn_Click(object sender, RoutedEventArgs e)
        {

        }

        string _Text = "";

        // 숫자 이거나 텍스트 형식일 경우 리턴 합니다.
        void txtinput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        // 숫자 이거나 텍스트 형식일 경우 리턴 합니다.
        public string Text
        {
            get { return _Text; }
        }


    }
}
