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
        private List<Field> playArea;
        public List<Field> PlayArea
        {
            get => playArea;
            set { playArea = value; NotifyPropertyChanged(); }
        }
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
            ChangeGameMode("easy"); // start with easy mode
        }
        private void ChangeGameMode(object o)
        {
            switch ((string)o)
            {
                case "easy":
                    PlayAreaSize = 8;
                    MineCount = FlagCount = 10;
                    break;
                case "medium":
                    PlayAreaSize = 16;
                    MineCount = FlagCount = 40;
                    break;
                case "hard":
                    PlayAreaSize = 22;
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
            PlayArea = GeneratePlayArea();
            GameState = Minesweeper.Enums.GameState.Running;
            Timer.Stop();
            GameTime = 0;
        }
        private List<Field> GeneratePlayArea()
        {
            List<Field> result = new List<Field>(PlayAreaSizeSquared);
            for (int i = 0; i < PlayAreaSizeSquared; ++i)
            {
                result.Add(new Field { Index = i });
            }
            return result;
        }
        private Field GetModel(FieldControl fieldControl) => PlayArea.Find(e => e.Index == fieldControl.FieldIndex);
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
            if (f.DangerLevel > 0 && !f.IsMine)
            {
                f.Covered = false;
                ExposedFields++;
            }
            else if (f.DangerLevel == 0)
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
                CheckVictory();
            }
        }
        private void CheckVictory()
        {
            if (ExposedFields == (PlayAreaSizeSquared - MineCount))
            {
                GameState = Enums.GameState.Victory;
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
    }
}
