using Minesweeper.Controls;
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
using Minesweeper.Models;

namespace Minesweeper {

    public partial class MainWindow : Window {
        public int PlayAreaSize { get; set; } = 10; // Rozmiar planszy
        public List<Field> PlayArea { get; set; }
        private int PlayAreaSizeSquared;
        public int MinesCount { get; set; } = 10; // Ilość min na planszy
        public MainWindow() {
            
            InitializeComponent();
            PlayAreaSizeSquared = PlayAreaSize * PlayAreaSize;
            PlayArea = new List<Field>(PlayAreaSizeSquared);
            generatePlayArea();
            placeMines();
            DataContext = this;
        }
        private void generatePlayArea()
        {
            for(int i = 0; i < PlayAreaSizeSquared; ++i)
            {
                PlayArea.Add(new Field { Index = i });
            }
        }
        private int GetProperIndex(int row, int col) => row * PlayAreaSize + col;
        private void placeMines()
        {
            var randomGenerator = new Random();

            for (int i = 0; i < MinesCount; ++i)
            {
                bool bombPlaced = false;
                while (!bombPlaced)
                {
                    var nextPlace = randomGenerator.Next(0, PlayAreaSizeSquared - 1); // zero-based index

                    var pickedField = PlayArea[nextPlace];

                    if(!pickedField.IsMine)
                    {
                        int currentColumn = nextPlace % PlayAreaSize;
                        int currentRow = nextPlace / PlayAreaSize;
                        // Place bomb
                        pickedField.IsMine = bombPlaced = true;

                        // Increase risk of neighbours
                        if(currentRow < (PlayAreaSize - 1))
                            PlayArea[GetProperIndex(currentRow + 1, currentColumn)].DangerLevel += 1;
                        if(currentRow > 0)
                            PlayArea[GetProperIndex(currentRow - 1, currentColumn)].DangerLevel += 1;
                        if(currentColumn < (PlayAreaSize - 1))
                            PlayArea[GetProperIndex(currentRow, currentColumn + 1)].DangerLevel += 1;
                        if (currentColumn > 0)
                            PlayArea[GetProperIndex(currentRow, currentColumn - 1)].DangerLevel += 1;
                        // Diagonal check
                        // Pomysł Kamila <3 Koksu <3
                        try
                        {
                            PlayArea[GetProperIndex(currentRow - 1, currentColumn - 1)].DangerLevel += 1;
                        }
                        catch(ArgumentOutOfRangeException) { };

                        try
                        {
                            PlayArea[GetProperIndex(currentRow - 1, currentColumn + 1)].DangerLevel += 1;
                        }
                        catch(ArgumentOutOfRangeException) { };
                        try
                        {
                            PlayArea[GetProperIndex(currentRow + 1, currentColumn - 1)].DangerLevel += 1;
                        }
                        catch(ArgumentOutOfRangeException) { };
                        try
                        {
                            PlayArea[GetProperIndex(currentRow + 1, currentColumn + 1)].DangerLevel += 1;
                        }
                        catch(ArgumentOutOfRangeException) { };
                    }

                }
                
            }

        }

        private void LeftButtonClick(object sender, RoutedEventArgs e)
        {
            var field = sender as FieldControl;
            if (field.FieldState == FieldStateTypes.Covered)
            {
                field.FieldState = FieldStateTypes.UnCovered;
                // TODO: Tutaj sprawdzamy czy kliknięto bombę field.IsMine :P
                
            }
        }
        private void RightButtonClick(object sender, RoutedEventArgs e)
        {
            var field = sender as FieldControl;
            if(field.Suspected)
            {
                field.Suspected = false;
                field.FieldState = FieldStateTypes.Covered;
            }
            else if(!field.Suspected)
            {
                field.Suspected = true;
                field.FieldState = FieldStateTypes.UnCovered;
            }
        }
    }
}