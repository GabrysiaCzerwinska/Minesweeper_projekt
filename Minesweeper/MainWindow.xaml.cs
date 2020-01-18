using System.Windows;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Board board = new Board(16, 48, grid);
        }
    }
}