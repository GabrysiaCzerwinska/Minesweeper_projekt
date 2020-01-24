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
using System.Windows.Threading;
using System.ComponentModel;
using System.Timers;

namespace Minesweeper {

    public enum GameState
    {
        Run,
        Defeat,
        Victory
    }


    public partial class MainWindow : Window, INotifyPropertyChanged {
        private int exposedFields = 0;
        private bool firstClick = false;
        private bool IsGameGoing = false;
        private int _playAreaSize;
        public int PlayAreaSize { get => _playAreaSize; set { _playAreaSize = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlayAreaSize")); } } // Rozmiar planszy
        private List<Field> _PlayArea;
        public List<Field> PlayArea { get => _PlayArea; set { _PlayArea = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlayArea")); } }
        private int PlayAreaSizeSquared;
        private ICommand _RadioButtonChecked;
        private GameState _gameState;
        public GameState gameState { get => _gameState; set { _gameState = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GameState")); } }
        public ICommand RadioButtonChecked
        {
            get => _RadioButtonChecked;
            set
            {
                _RadioButtonChecked = value;
            }
        }

        private ICommand _ClickFaceButton;

        public ICommand ClickFaceButton
        {
            get => _ClickFaceButton;
            set
            {
                _ClickFaceButton = value;
            }
        }
        private ICommand _RightFieldClick;

        public ICommand RightFieldClick
        {
            get => _RightFieldClick;
            set
            {
                _RightFieldClick = value;
            }
        }

        private ICommand _LeftFieldClick;

        public ICommand LeftFieldClick
        {
            get => _LeftFieldClick;
            set
            {
                _LeftFieldClick = value;
            }
        }
        private readonly Timer timer;

        public event PropertyChangedEventHandler PropertyChanged;
        private int _flagCount;
        public int MinesCount { get; set; }  // Ilość min na planszy

        public int FlagCount { get => _flagCount; set { _flagCount = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagCount")); } }
        //private DispatcherTimer timer { get; set; }

        private int _gameTime = 0;

        public int GameTime { get => _gameTime; set { _gameTime = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GameTime")); } } 
        public MainWindow() {
            
            InitializeComponent();
            RadioButtonChecked = new RelayCommand(ChangeGameMode, param => !IsGameGoing);
            ClickFaceButton = new RelayCommand(Button_Click, param => true);
            LeftFieldClick = new RelayCommand(LeftButtonClick, param => true);
            RightFieldClick = new RelayCommand(RightButtonClick, param => true);
            timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            timer.Elapsed += Timer_Tick;
            DataContext = this;
            ChangeGameMode("easy");
            
        }

        private void ChangeGameMode(object mode)
        {
           switch((string)mode)
            {
                case "easy":
                    PlayAreaSize = 8;
                    MinesCount = FlagCount = 10;
                    break;
                case "medium":
                    PlayAreaSize = 16;
                    MinesCount = FlagCount = 40;
                    break;
                case "hard":
                    PlayAreaSize = 22;
                    MinesCount = FlagCount = 100;
                    break;
            }
            resetGame();
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {

            GameTime += 1;
        }

        private List<Field> GeneratePlayArea()
        {
            List<Field> result = new List<Field>(PlayAreaSizeSquared);
            for(int i = 0; i < PlayAreaSizeSquared; ++i)
            {
                result.Add(new Field { Index = i });
            }
            return result;
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

                    if(!pickedField.IsMine && pickedField.Covered && !pickedField.FirstClicked)
                    {
                        int currentColumn = nextPlace % PlayAreaSize;
                        int currentRow = nextPlace / PlayAreaSize;
                        // Place bomb
                        pickedField.IsMine = bombPlaced = true;
                    }

                }
                
            }
            setNeighboursDangerLevel();

        }

        private void setNeighboursDangerLevel()
        {
            foreach(var field in PlayArea)
            {
                if (field.IsMine)
                {
                    int col = field.Index % PlayAreaSize;
                    int row = field.Index / PlayAreaSize;

                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                        {
                            if ((row + i < 0) || (row + i >= PlayAreaSize) || (col + j < 0) || (col + j >= PlayAreaSize))
                                continue;
                            var f = PlayArea[GetProperIndex(row + i, col + j)];
                            if (!f.IsMine && f.Covered)
                                f.increaseDangerLevel();
                        }
                }
            }
        }

        private void UncoverAllFields()
        {
            foreach(var t in PlayArea)
            {
                if(t.IsSuspected)
                {
                    t.IsSuspected = false;
                }
                t.Covered = false;
            }
        }

        private Field getModel(FieldControl field)
        {
            return PlayArea.Find(elem => elem.Index == field.FieldIndex);
        }


        private void RevealUncoveredFields(Field f)
        {
            if (f.DangerLevel > 0 && !f.IsMine)
            {
                f.Covered = false;
                exposedFields++;
            }
            else if (f.DangerLevel == 0)
            {
                f.Covered = false;
                exposedFields++;
                int col = f.Index % PlayAreaSize;
                int row = f.Index / PlayAreaSize;
                for (int i = -1; i < 2; i++)
                    for (int j = -1; j < 2; j++)
                    {
                        if ((row + i < 0) || (row + i >= PlayAreaSize) || (col + j < 0) || (col + j >= PlayAreaSize))
                            continue;
                        var field = PlayArea[GetProperIndex(row + i, col + j)];
                        if (field.Covered && !field.IsSuspected)
                            RevealUncoveredFields(field);
                    }
            }
        }
        private void LeftButtonClick(object sender)
        {
            var field = sender as FieldControl;
            if (field.FieldCovered)
            {
                var m = getModel(field);
                if(!firstClick)
                {
                    gameState = GameState.Run;
                    IsGameGoing = true;
                    m.FirstClicked = true;
                    placeMines();
                    if (!timer.Enabled)
                    {
                        timer.Start();
                    }
                    firstClick = true;
                }
                if (m.IsMine)
                {
                    gameState = GameState.Defeat;
                    timer.Stop();
                    m.Covered = false;
                    IsGameGoing = false;
                    UncoverAllFields();
                }
                else
                {
                    RevealUncoveredFields(m);
                }

                if(exposedFields == PlayAreaSizeSquared - MinesCount)
                {
                    gameState = GameState.Victory;
                    timer.Stop();
                }

            }
        }
        private void RightButtonClick(object sender)
        {   if (firstClick)
            {
                var f = getModel(sender as FieldControl);

                if(f.Covered && FlagCount > 0)
                {
                    f.IsSuspected = true;
                    FlagCount--;
                }
                else if(f.IsSuspected)
                {
                    f.IsSuspected = false;
                    FlagCount++;
                }
            }
        }
        private void resetGame()
        {
            PlayAreaSizeSquared = PlayAreaSize * PlayAreaSize;
            PlayArea = GeneratePlayArea();
            firstClick = false;
            IsGameGoing = false;
            FlagCount = MinesCount;
            gameState = GameState.Run;
            timer.Stop();
            GameTime = 0;
        }
        private void Button_Click(object sender)
        {
            resetGame();
        }
    }
}