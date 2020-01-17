using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minesweeper.Controls
{
    /// <summary>
    /// Interaction logic for FieldControl.xaml
    /// </summary>
    public partial class FieldControl : UserControl
    {
        public FieldControl()
        {
            InitializeComponent();
           // DataContext = this;
        }
        public static readonly DependencyProperty InnerContentProperty = DependencyProperty.Register(
            "InnerContent", typeof(object), typeof(FieldControl), new PropertyMetadata(null, OnSetTextChanged));
        public object InnerContent { get => GetValue(InnerContentProperty); set => SetValue(InnerContentProperty, value); }

        public static readonly DependencyProperty RightClickProperty = DependencyProperty.Register(
            "RightClick", typeof(RoutedEventHandler), typeof(FieldControl), new PropertyMetadata());
        public RoutedEventHandler RightClick { get => (RoutedEventHandler)GetValue(RightClickProperty); set => SetValue(RightClickProperty, value); }
        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
            "Click", typeof(RoutedEventHandler), typeof(FieldControl), new PropertyMetadata());
        public RoutedEventHandler Click { get => (RoutedEventHandler)GetValue(ClickProperty); set => SetValue(ClickProperty, value); }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(SolidColorBrush), typeof(FieldControl), new PropertyMetadata(Brushes.Black));
        public SolidColorBrush Color { get => (SolidColorBrush)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(double), typeof(FieldControl), new PropertyMetadata());
        public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

        private void Button_Click(object sender, RoutedEventArgs e) => Click?.Invoke(this, e);
        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e) => RightClick?.Invoke(this, e);


        private static void OnSetTextChanged(DependencyObject d,
        DependencyPropertyChangedEventArgs e)
        {
            FieldControl UserControl1Control = d as FieldControl;
            //UserControl1Control.OnSetTextChanged(e);
        }
    }
}
