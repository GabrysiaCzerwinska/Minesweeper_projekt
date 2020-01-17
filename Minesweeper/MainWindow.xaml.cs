using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Minesweeper {
    public class TestClass
    {
        public string TestString { get; set; }
    }
    public partial class MainWindow : Window {
        public int playAreaHeight { get; set; } = 10;
        public int playAreaWidth { get; set; } = 10;
        public List<TestClass> testGrid { get; set; } = new List<TestClass>();

        public MainWindow() {
            generateTestGrid();
            InitializeComponent();
            DataContext = this;
        }
        private void generateTestGrid()
        {
            for(int i = 1; i <= playAreaHeight * playAreaWidth; ++i)
            {
                testGrid.Add(new TestClass { TestString = i.ToString() });
            }
        }

        private void Test1(object sender, RoutedEventArgs e)
        {
            var dupa = (sender as Minesweeper.Controls.FieldControl);
            Debug.WriteLine("Matko działa!");
        }
        private void Test2(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Matko działa drugi przycisk!");
        }
    }
}