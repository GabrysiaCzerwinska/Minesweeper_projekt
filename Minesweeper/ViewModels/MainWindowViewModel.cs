using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Minesweeper.Models;
using Minesweeper.Enums;
using System.Windows.Input;
using Minesweeper.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Minesweeper.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region ViewModel Fields
        private int ExposedFields;
        private bool IsGameRunning;
        private readonly System.Timers.Timer Timer;
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
        #region PlayArea Property
        public ObservableCollection<Field> PlayArea { get; set; }
        #endregion
        #region PlayAreaSize Property
        private int playAreaSize;
        public int PlayAreaSize
        {
            get => playAreaSize;
            set { playAreaSize = value; NotifyPropertyChanged(); }
        }
        #endregion
        // Contains Squared PlayAreaSize
        private int PlayAreaSizeSquared;
        #region GameState Property
        private Minesweeper.Enums.GameState gameState;
        public Minesweeper.Enums.GameState GameState
        {
            get => gameState;
            set { gameState = value; NotifyPropertyChanged(); }
        }
        #endregion
        #region GameTime Property
        private int gameTime;
        public int GameTime
        {
            get => gameTime;
            set { gameTime = value; NotifyPropertyChanged(); }
        }
        #endregion
        private int MineCount;
        #region FieldSize Property
        private double fieldSize;
        public double FieldSize { get => fieldSize; set { fieldSize = value; NotifyPropertyChanged(); } }
        #endregion
        #region FieldMargin Property
        private double fieldMargin;
        public double FieldMargin { get => fieldMargin; set { fieldMargin = value; NotifyPropertyChanged(); } }
        #endregion
        #region FlagCount Property
        private int flagCount;
        public int FlagCount
        {
            get => flagCount;
            set { flagCount = value; NotifyPropertyChanged(); }
        }
        #endregion
        #region Button Commands
        public ICommand DifficultyLevelChange { get; set; }
        public ICommand LeftButtonFieldClick { get; set; }
        public ICommand RightButtonFieldClick { get; set; }
        public ICommand FaceButtonClick { get; set; }
        public ICommand FieldButtonDown { get; set; }
        #endregion
        #endregion
        public MainWindowViewModel()
        {
            Timer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            Timer.Elapsed += (object o, ElapsedEventArgs e) => { GameTime += 1; };
            DifficultyLevelChange = new RelayCommand(ChangeGameMode, o => !IsGameRunning);
            LeftButtonFieldClick = new RelayCommand(LeftButtonField_Click);
            RightButtonFieldClick = new RelayCommand(RightButtonField_Click);
            FaceButtonClick = new RelayCommand(o => ResetGame());
            FieldButtonDown = new RelayCommand(LeftButtonField_Down, o => IsGameRunning);
            PlayArea = new ObservableCollection<Field>();
            ChangeGameMode("easy"); // start with easy mode
        }
        private void ChangeGameMode(object o)
        {
            switch ((string)o)
            {
                case "easy":
                    PlayAreaSize = 8;
                    FieldMargin = 1;
                    FieldSize = 34;
                    MineCount = FlagCount = 10;
                    break;
                case "medium":
                    PlayAreaSize = 16;
                    FieldMargin = 1;
                    FieldSize = 23.5;
                    MineCount = FlagCount = 40;
                    break;
                case "hard":
                    PlayAreaSize = 22;
                    FieldMargin = .5;
                    FieldSize = 20.5;
                    MineCount = FlagCount = 100;
                    break;
            }
            ResetGame();
        }
        private void ResetGame()
        {
            PlayAreaSizeSquared = (int)Math.Pow(PlayAreaSize, 2);
            IsGameRunning = false;
            FlagCount = MineCount;
            ExposedFields = 0;
            ResizePlayArea();
            ResetFields();
            GameState = Minesweeper.Enums.GameState.Running;
            Timer.Stop();
            GameTime = 0;
        }
        private void ResetFields()
        {
            foreach (Field f in PlayArea)
                f.Reset();
        }
        private void ResizePlayArea()
        {
            int difference = PlayAreaSizeSquared - PlayArea.Count();
            if (difference > 0)
            {
                int index = 0;
                var lastElem = PlayArea.LastOrDefault();
                if (lastElem != null)
                {
                    index = lastElem.Index + 1;
                }
                while (difference > 0)
                {
                    PlayArea.Add(new Field { Index = index });
                    difference--;
                    index++;
                }
            }
            else if (difference < 0)
            {
                while(difference < 0)
                {
                    PlayArea.RemoveAt(PlayArea.Count() - 1);
                    difference++;
                }
            }
        }
        private Field GetModel(FieldControl fieldControl) => PlayArea.Where(e => e.Index == fieldControl.FieldIndex).First();
        private void PlaceMines()
        {
            Random randomGenerator = new Random();

            for (int i = 0; i < MineCount; ++i)
            {
                bool bombPlaced = false;
                while (!bombPlaced)
                {
                    var nextPlace = randomGenerator.Next(0, PlayAreaSizeSquared - 1); // zero-based index

                    var pickedField = PlayArea[nextPlace];

                    if (!pickedField.IsMine && pickedField.Covered && !pickedField.FirstClicked)
                    {
                        // Place bomb
                        pickedField.IsMine = bombPlaced = true;
                        SetNeighboursDangerLevel(pickedField);
                    }

                }
            }
        }
        private int Get1DIndex(int row, int column) => row * PlayAreaSize + column;
        private void SetNeighboursDangerLevel(Field field)
        {
            if(field.IsMine)
            {
                int col = field.Index % PlayAreaSize;
                int row = field.Index / PlayAreaSize;

                for (int i = -1; i < 2; i++)
                    for (int j = -1; j < 2; j++)
                    {
                        if ((row + i < 0) || (row + i >= PlayAreaSize) || (col + j < 0) || (col + j >= PlayAreaSize))
                            continue;
                        var f = PlayArea[Get1DIndex(row + i, col + j)];
                        if (!f.IsMine && f.Covered)
                            f.IncreaseDangerLevel();
                    }
            }
        }
        private void ShowAllFields()
        {
            foreach (Field field in PlayArea)
            {
                if (field.IsSuspected)
                    field.IsSuspected = false;
                field.Covered = false;
            }
        }
        private void ShowCoveredFields(Field f)
        {
            if (f.DangerLevel > 0 && !f.IsMine && f.Covered)
            {
                f.Covered = false;
                ExposedFields++;
            }
            else if (f.DangerLevel == 0 && f.Covered)
            {
                f.Covered = false;
                ExposedFields++;
                int col = f.Index % PlayAreaSize;
                int row = f.Index / PlayAreaSize;
                for (int i = -1; i < 2; i++)
                    for (int j = -1; j < 2; j++)
                    {
                        if ((row + i < 0) || (row + i >= PlayAreaSize) || (col + j < 0) || (col + j >= PlayAreaSize))
                            continue;
                        var field = PlayArea[Get1DIndex(row + i, col + j)];
                        if (field.Covered && !field.IsSuspected)
                            ShowCoveredFields(field);
                    }
            }
        }

        private void LeftButtonField_Click(object sender)
        {
            GameState = Minesweeper.Enums.GameState.Running;
            Field field = GetModel(sender as FieldControl);
            if (field.Covered)
            {
                if (!IsGameRunning)
                {
                    IsGameRunning = true;
                    field.FirstClicked = true; // mark as first clicked
                    PlaceMines();
                    Timer.Start();
                }
                if(field.IsMine)
                {
                    GameState = Enums.GameState.Defeat;
                    Timer.Stop();
                    field.Covered = false;
                    IsGameRunning = false;
                    ShowAllFields();
                }
                else
                {
                    ShowCoveredFields(field);
                }
            }
            CheckVictory();
        }
        private void CheckVictory()
        {
            if (ExposedFields == (PlayAreaSizeSquared - MineCount))
            {
                GameState = Enums.GameState.Victory;
                IsGameRunning = false;
                Timer.Stop();
            }
        }
        private void RightButtonField_Click(object sender)
        {
            if(IsGameRunning)
            {
                Field field = GetModel(sender as FieldControl);
                if (field.Covered && FlagCount > 0)
                {
                    field.IsSuspected = true;
                    FlagCount--;
                }
                else if (field.IsSuspected)
                {
                    field.IsSuspected = false;
                    FlagCount++;
                }
                CheckVictory();
            }
        }
        private void LeftButtonField_Down(object sender)
        {
            if(GetModel(sender as FieldControl).Covered)
                GameState = Minesweeper.Enums.GameState.UserClick;
        }
    }
}
