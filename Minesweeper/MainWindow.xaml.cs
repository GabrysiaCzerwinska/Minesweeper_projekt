using System.Windows;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Board board = new Board(10, 10, grid);
        }
    }
}