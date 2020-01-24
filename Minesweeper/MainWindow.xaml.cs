using System;
using System.Timers;
using System.Windows;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        public TimeSpan gameLength;
        public DateTime todaysDate;
        public Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            normal.Click += new RoutedEventHandler(clickNormal);
            easy.Click += new RoutedEventHandler(clickEasy);
            hard.Click += new RoutedEventHandler(clickHard);

            gameLength = new TimeSpan(TimeSpan.Zero.Days, TimeSpan.Zero.Hours, TimeSpan.Zero.Minutes, TimeSpan.Zero.Seconds);
            todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            timer = new Timer(1000);

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }
        public void OnTimedEvent(Object sender, ElapsedEventArgs e)
        {
            gameLength = new DateTime(e.SignalTime.Year, e.SignalTime.Month, e.SignalTime.Day, e.SignalTime.Hour, e.SignalTime.Minute, e.SignalTime.Second) - todaysDate;

            TimerBox.Dispatcher.Invoke(() => { TimerBox.Text = gameLength.ToString(); });
        }



        private void clickNormal(object sender, RoutedEventArgs e)
        {
            Board board = new Board(16, 44, grid);
        }
        private void clickEasy(object sender, RoutedEventArgs e)
        {
            Board board = new Board(8, 10, grid);
        }
        private void clickHard(object sender, RoutedEventArgs e)
        {
            Board board = new Board(24, 100, grid);
        }


    }
}