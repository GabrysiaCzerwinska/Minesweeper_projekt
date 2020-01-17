using Mineswepper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// 

    public enum FieldStateTypes
    {
        Covered,
        UnCovered,
    }

    public partial class FieldControl : UserControl, INotifyPropertyChanged
    {
        private int previousValue = -1;
        public FieldControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FieldStateProperty = DependencyProperty.Register(
            "FieldState", typeof(FieldStateTypes), typeof(FieldControl), new PropertyMetadata(FieldStateTypes.Covered));
        public FieldStateTypes FieldState
        {
            get => (FieldStateTypes)GetValue(FieldStateProperty);
            set
            {
                SetValue(FieldStateProperty, value);
                NotifyPropertyChanged();
            }
        }
        public static readonly DependencyProperty FieldValueProperty = DependencyProperty.Register(
            "FieldValue", typeof(int), typeof(FieldControl), new PropertyMetadata(0));
        public int FieldValue
        {
            get => (int)GetValue(FieldValueProperty);
            set { 
                SetValue(FieldValueProperty, value);
                NotifyPropertyChanged();
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty RightClickProperty = DependencyProperty.Register(
            "RightClick", typeof(RoutedEventHandler), typeof(FieldControl), new PropertyMetadata());
        public RoutedEventHandler RightClick { get => (RoutedEventHandler)GetValue(RightClickProperty); set => SetValue(RightClickProperty, value); }
        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
            "Click", typeof(RoutedEventHandler), typeof(FieldControl), new PropertyMetadata());
        public RoutedEventHandler Click { get => (RoutedEventHandler)GetValue(ClickProperty); set => SetValue(ClickProperty, value); }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(double), typeof(FieldControl), new PropertyMetadata());

        public event PropertyChangedEventHandler PropertyChanged;

        public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }


        public static readonly DependencyProperty FieldIndexProperty = DependencyProperty.Register(
            "FieldIndex", typeof(int), typeof(FieldControl), new PropertyMetadata());

        public int FieldIndex
        {
            get => (int)GetValue(FieldIndexProperty);
            set => SetValue(FieldIndexProperty, value);
        }
        public static readonly DependencyProperty FieldCoveredProperty = DependencyProperty.Register(
            "FieldCovered", typeof(bool), typeof(FieldControl), new PropertyMetadata(true, new PropertyChangedCallback(OnDependencyPropertyChanged)));

        public bool FieldCovered { 
            get => (bool)GetValue(FieldCoveredProperty);
            set
            {
                if (value)
                {
                    FieldState = FieldStateTypes.Covered;
                }
                else
                {
                    FieldState = FieldStateTypes.UnCovered;
                }
                SetValue(FieldCoveredProperty, value);
            } 
          } 


        public static readonly DependencyProperty IsMineProperty = DependencyProperty.Register(
            "IsMine", typeof(bool), typeof(FieldControl), new PropertyMetadata(false, new PropertyChangedCallback(OnDependencyPropertyChanged)));
        public bool IsMine
        {
            get => FieldValue == 1000;
            set
            {
                if (value)
                    FieldValue = 1000;
            }
        }
        private static void OnDependencyPropertyChanged(DependencyObject d,
        DependencyPropertyChangedEventArgs e)
        {
            var _this = (FieldControl)d;
            if(e.Property.Name == "IsMine")
            {
                _this.CiulWie(e);
            }
            if(e.Property.Name == "FieldCovered")
            {
                _this.ChangeFieldCovered(e);
            }

        }

        private void CiulWie(DependencyPropertyChangedEventArgs e)
        {
            IsMine = (bool)e.NewValue;
        }

        private void ChangeFieldCovered(DependencyPropertyChangedEventArgs e)
        {
            FieldCovered = (bool)e.NewValue;
        }

        public static readonly DependencyProperty DuspectedProperty = DependencyProperty.Register(
            "Suspected", typeof(bool), typeof(FieldControl), new PropertyMetadata(false));
        public bool Suspected
        {
            get => FieldValue == 1001;
            set
            {
                if (!value)
                {
                    if (previousValue != -1)
                    {
                        FieldValue = previousValue;
                        previousValue = -1;
                    }
                }
                else
                {
                    if (FieldState == FieldStateTypes.Covered)
                    {
                        previousValue = FieldValue;
                        FieldValue = 1001;
                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e) => Click?.Invoke(this, e);
        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e) => RightClick?.Invoke(this, e);
    }
}
