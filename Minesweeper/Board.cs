using Mineswepper;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Minesweeper
{
    class Board
    {
        public int size;
        public int mineNumber;
        public int exposedFieldsNumber;
        public TextBlock statusBlock;
        public Timer timer;
        public Grid boardGrid;
        public Field[,] board;

        public Board(int _size, int _mineNumber, TextBlock _statusBlock, Timer _timer, Grid _boardGrid)
        {
            size = _size;
            mineNumber = _mineNumber;
            exposedFieldsNumber = 0;
            statusBlock = _statusBlock;
            timer = _timer;
            boardGrid = _boardGrid;

            board = new Field[size, size];

            createBoard();
            drawBoard();
        }

        public void createBoard()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    board[i, j] = new Field();

            int k = mineNumber;
            Random random = new Random();

            while (k > 0)
            {
                int row = random.Next(0, size);
                int column = random.Next(0, size);

                Field field = board[row, column];

                if (!field.isMined)
                {
                    field.mine();

                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                        {
                            if ((row + i < 0) || (row + i >= size) || (column + j < 0) || (column + j >= size))
                                continue;

                            board[(row + i), (column + j)].increaseDangerLevel();
                        }

                    k--;
                }
            }
        }

        public void drawBoard()
        {
            boardGrid.Children.Clear();
            boardGrid.RowDefinitions.Clear();
            boardGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    boardGrid.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = new GridLength()
                    });

                    boardGrid.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength()
                    });

                    Button button = new Button()
                    {
                        Tag = $"{ i }|{ j }",
                        Width = 24,
                        Height = 24,
                        Margin = new Thickness(2),
                        BorderThickness = new Thickness(1),
                        Background = Brushes.Transparent
                    };

                    Field field = board[i, j];

                    if (field.isExposed)
                    {
                        if (field.isMined)
                        {
                            button.Content = "X";
                            button.Foreground = Brushes.Crimson;
                            button.BorderBrush = Brushes.Crimson;
                        }
                        else
                        {
                            switch (field.dangerLevel)
                            {
                                case 0:
                                    button.BorderBrush = Brushes.MidnightBlue;
                                    break;
                                case 1:
                                    button.Content = field.dangerLevel;
                                    button.Foreground = Brushes.Orchid;
                                    button.BorderBrush = Brushes.Orchid;
                                    break;
                                case 2:
                                    button.Content = field.dangerLevel;
                                    button.Foreground = Brushes.MediumOrchid;
                                    button.BorderBrush = Brushes.MediumOrchid;
                                    break;
                                case 3:
                                    button.Content = field.dangerLevel;
                                    button.Foreground = Brushes.DarkOrchid;
                                    button.BorderBrush = Brushes.DarkOrchid;
                                    break;
                                default:
                                    button.Content = field.dangerLevel;
                                    button.Foreground = Brushes.DarkMagenta;
                                    button.BorderBrush = Brushes.DarkMagenta;
                                    break;
                            }
                        }
                    }
                    else if (field.isSuspected)
                    {
                        button.Content = "?";
                        button.Foreground = Brushes.Aquamarine;
                        button.BorderBrush = Brushes.Aquamarine;
                    }
                    else
                    {
                        button.BorderBrush = Brushes.White;
                    }

                    button.Click += new RoutedEventHandler(handleLeftMouseButtonClick);
                    button.MouseDown += new MouseButtonEventHandler(handleRightMouseButtonClick);

                    boardGrid.Children.Add(button);

                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                }
            }
        }


        public void exposeHiddenFields(int row, int column)
        {
            Field field = board[row, column];

            if ((field.dangerLevel == 0) && !field.isMined && !field.isSuspected && !field.isExposed)
            {
                field.expose();

                exposedFieldsNumber++;

                for (int i = -1; i < 2; i++)
                    for (int j = -1; j < 2; j++)
                    {
                        if ((row + i < 0) || (row + i >= size) || (column + j < 0) || (column + j >= size))
                            continue;

                        field = board[(row + i), (column + j)];

                        if ((field.dangerLevel == 0) && !field.isMined && !field.isSuspected && !field.isExposed)
                        {
                            exposeHiddenFields((row + i), (column + j));
                        }
                        else if (!field.isSuspected && !field.isExposed)
                        {
                            field.expose();

                            exposedFieldsNumber++;
                        }
                    }
            }
        }

        public void exposeAllFields()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    board[i, j].expose();
        }

        public void handleLeftMouseButtonClick(object sender, RoutedEventArgs e)
        {
            string[] position = ((Button)sender).Tag.ToString().Split('|');

            int row = Int32.Parse(position[0]);
            int column = Int32.Parse(position[1]);

            Field field = board[row, column];

            if (!field.isSuspected)
            {
                if (field.isMined)
                {
                    exposeAllFields();

                    statusBlock.Foreground = Brushes.Red;
                    statusBlock.Text = "You lost!";

                    timer.Stop();
                }
                else if (field.dangerLevel > 0)
                {
                    field.expose();

                    exposedFieldsNumber++;
                }
                else
                {
                    exposeHiddenFields(row, column);
                }

                if (exposedFieldsNumber == ((size * size) - mineNumber))
                {
                    exposeAllFields();

                    statusBlock.Foreground = Brushes.Green;
                    statusBlock.Text = "You win!";

                    timer.Stop();
                }
            }

            drawBoard();
        }

        public void handleRightMouseButtonClick(object sender, MouseButtonEventArgs e)
        {
            string[] position = ((Button)sender).Tag.ToString().Split('|');

            board[Int32.Parse(position[0]), Int32.Parse(position[1])].changeSuspect();

            drawBoard();
        }
    }
}