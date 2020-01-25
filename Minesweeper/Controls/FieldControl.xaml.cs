using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class FieldControl : UserControl, INotifyPropertyChanged
    {
        private int previousValue = -1;
        public FieldControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FieldValueProperty = DependencyProperty.Register(
            "FieldValue", typeof(int), typeof(FieldControl), new PropertyMetadata(0, new PropertyChangedCallback(OnDependencyPropertyChanged)));
        public int FieldValue
        {
            get => (int)GetValue(FieldValueProperty);
            set { 
                SetValue(FieldValueProperty, value);
                NotifyPropertyChanged();
            }
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
        #region Size property
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(double), typeof(FieldControl), new PropertyMetadata());
        public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }
        #endregion
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
                SetValue(FieldCoveredProperty, value);
                NotifyPropertyChanged();
            } 
          } 
        public static readonly DependencyProperty IsMineProperty = DependencyProperty.Register(
            "IsMine", typeof(bool), typeof(FieldControl), new PropertyMetadata(false, new PropertyChangedCallback(OnDependencyPropertyChanged)));
        public bool IsMine { get => (bool)GetValue(IsMineProperty); set => SetValue(IsMineProperty, value); }
        private static void OnDependencyPropertyChanged(DependencyObject d,
        DependencyPropertyChangedEventArgs e)
        {
            var _this = (FieldControl)d;

            switch (e.Property.Name)
            {
                case "IsMine":
                    if ((bool)e.NewValue)
                        _this.FieldValue = 10000;
                    break;
                case "FieldCovered":
                    _this.FieldCovered = (bool)e.NewValue;
                    break;
                case "FieldValue":
                    _this.FieldValue = (int)e.NewValue;
                    break;
                case "Suspected":
                    _this.FieldCovered = !(bool)e.NewValue;
                    if (_this.FieldValue == 1001)
                    {
                        _this.FieldValue = _this.previousValue;
                        _this.previousValue = -1;
                    }
                    else
                    {
                        _this.previousValue = _this.FieldValue;
                        _this.FieldValue = 1001;
                    }
                    break;
            }
        }
        public static readonly DependencyProperty SuspectedProperty = DependencyProperty.Register(
            "Suspected", typeof(bool), typeof(FieldControl), new PropertyMetadata(false, new PropertyChangedCallback(OnDependencyPropertyChanged)));
        public bool Suspected { get => (bool)GetValue(SuspectedProperty); set => SetValue(SuspectedProperty, value); }

        #region LeftClick
        public static readonly DependencyProperty LeftClickCommandProperty = DependencyProperty.Register(
            "LeftClickCommand", typeof(ICommand), typeof(FieldControl), new PropertyMetadata());

        public ICommand LeftClickCommand
        {
            get => (ICommand)GetValue(LeftClickCommandProperty);
            set => SetValue(LeftClickCommandProperty, value);
        }

        private void Button_LeftClick(object sender, RoutedEventArgs e) => LeftClickCommand.Execute(this);
        #endregion

        #region RightClick
        public static readonly DependencyProperty RightClickCommandProperty = DependencyProperty.Register(
            "RightClickCommand", typeof(ICommand), typeof(FieldControl), new PropertyMetadata());
        public ICommand RightClickCommand
        {
            get => (ICommand)GetValue(RightClickCommandProperty);
            set => SetValue(RightClickCommandProperty, value);
        }

        private void Button_RightClick(object sender, MouseButtonEventArgs e) => RightClickCommand.Execute(this);
        #endregion
        #region Right Button Down
        public static readonly DependencyProperty LeftButtonDownCommandProperty = DependencyProperty.Register(
            "LeftButtonDownCommand", typeof(ICommand), typeof(FieldControl), new PropertyMetadata());
        public ICommand LeftButtonDownCommand
        {
            get => (ICommand)GetValue(LeftButtonDownCommandProperty);
            set => SetValue(LeftButtonDownCommandProperty, value);
        }
        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => LeftButtonDownCommand.Execute(this);
        #endregion
    }
}
