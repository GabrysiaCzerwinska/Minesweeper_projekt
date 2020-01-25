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
        private Board board;

        public MainWindow()
        {
            InitializeComponent();

            beginnerButton.Click += new RoutedEventHandler(clickBeginnerButton);
            intermediateButton.Click += new RoutedEventHandler(clickIntermediateButton);
            expertButton.Click += new RoutedEventHandler(clickExpertButton);

            gameLength = new TimeSpan(TimeSpan.Zero.Days, TimeSpan.Zero.Hours, TimeSpan.Zero.Minutes, TimeSpan.Zero.Seconds);
        }
        public void OnTimedEvent(Object sender, ElapsedEventArgs e)
        {
            gameLength = new DateTime(e.SignalTime.Year, e.SignalTime.Month, e.SignalTime.Day, e.SignalTime.Hour, e.SignalTime.Minute, e.SignalTime.Second) - todaysDate;

            timerBox.Dispatcher.Invoke(() =>
            {
                timerBox.Text = gameLength.ToString();
            });
        }

        private void clickBeginnerButton(object sender, RoutedEventArgs e)
        {
            statusBox.Text = "";

            todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            timer = new Timer(1000);

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;

            board = new Board(8, 10, statusBox, boardGrid);
        }

        private void clickIntermediateButton(object sender, RoutedEventArgs e)
        {
            statusBox.Text = "";

            todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            timer = new Timer(1000);

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;

            board = new Board(16, 40, statusBox, boardGrid);
        }

        private void clickExpertButton(object sender, RoutedEventArgs e)
        {
            statusBox.Text = "";

            todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            timer = new Timer(1000);

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;

            board = new Board(21, 100, statusBox, boardGrid);
        }
    }
}