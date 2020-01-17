using Mineswepper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Minesweeper {
    class Board {
        public int size;
        public int bombNumber;
        public Grid grid;
        public Field[,] board;

        public Board(int _size, int _bombNumber, Grid _grid) {
            size = _size;
            bombNumber = _bombNumber;
            grid = _grid;

            board = new Field[size, size];

            createBoard();
            drawBoard();
        }

        public void createBoard() {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    board[i, j] = new Field();

            for (int i = 0; i < bombNumber; i++) {
                int row = new Random().Next(0, size);
                int column = new Random().Next(0, size);

                if (board[row, column].isBombed) {
                    i--;
                } else {
                    board[row, column].bomb();

                    try {
                        board[(row - 1), (column - 1)].increaseDangerLevel();
                    } catch { }

                    try {
                        board[(row - 1), column].increaseDangerLevel();
                    } catch { }

                    try {
                        board[(row - 1), (column + 1)].increaseDangerLevel();
                    } catch { }

                    try {
                        board[row, (column - 1)].increaseDangerLevel();
                    } catch { }

                    try {
                        board[row, (column + 1)].increaseDangerLevel();
                    } catch { }

                    try {
                        board[(row + 1), (column - 1)].increaseDangerLevel();
                    } catch { }

                    try {
                        board[(row + 1), column].increaseDangerLevel();
                    } catch { }

                    try {
                        board[(row + 1), (column + 1)].increaseDangerLevel();
                    } catch { }
                }
            }
        }

        public void drawBoard() {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    grid.RowDefinitions.Add(new RowDefinition() {
                        Height = new GridLength()
                    });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() {
                        Width = new GridLength()
                    });

                    Field field = board[i, j];

                    Button button = new Button() {
                        Width = 24,
                        Height = 24,
                        Margin = new Thickness(2),
                        BorderThickness = new Thickness(1),
                        Background = Brushes.Transparent
                    };

                    if (field.isExposed) {
                        if (field.isBombed) {
                            button.Content = "X";
                            button.Foreground = Brushes.Crimson;
                            button.BorderBrush = Brushes.Crimson;
                            button.Focusable = false;
                        } else {
                            switch (field.dangerLevel) {
                                case 0:
                                    button.BorderBrush = Brushes.MidnightBlue;
                                    button.Focusable = false;
                                    break;
                                case 1:
                                    button.Content = field.dangerLevel;
                                    button.Foreground = Brushes.Orchid;
                                    button.BorderBrush = Brushes.Orchid;
                                    button.Focusable = false;
                                    break;
                                case 2:
                                    button.Content = field.dangerLevel;
                                    button.Foreground = Brushes.MediumOrchid;
                                    button.BorderBrush = Brushes.MediumOrchid;
                                    button.Focusable = false;
                                    break;
                                case 3:
                                    button.Content = field.dangerLevel;
                                    button.Foreground = Brushes.DarkOrchid;
                                    button.BorderBrush = Brushes.DarkOrchid;
                                    button.Focusable = false;
                                    break;
                                default:
                                    button.Content = field.dangerLevel;
                                    button.Foreground = Brushes.DarkMagenta;
                                    button.BorderBrush = Brushes.DarkMagenta;
                                    button.Focusable = false;
                                    break;
                            }
                        }
                    } else if (field.isSuspected) {
                        button.Content = "?";
                        button.Foreground = Brushes.Aquamarine;
                        button.BorderBrush = Brushes.Aquamarine;
                    } else {
                        button.BorderBrush = Brushes.White;
                    }

                    button.Tag = $"{ i }|{ j }";
                    button.Click += new RoutedEventHandler(expose);
                    button.MouseDown += new MouseButtonEventHandler(suspect);

                    grid.Children.Add(button);

                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                }
            }
        }

        public void expose(object sender, RoutedEventArgs e) {
            string[] position = ((Button)sender).Tag.ToString().Split('|');

            board[Int32.Parse(position[0]), Int32.Parse(position[1])].expose();

            drawBoard();
        }

        public void suspect(object sender, MouseButtonEventArgs e) {
            string[] position = ((Button)sender).Tag.ToString().Split('|');

            board[Int32.Parse(position[0]), Int32.Parse(position[1])].suspect();

            drawBoard();
        }
    }
}
