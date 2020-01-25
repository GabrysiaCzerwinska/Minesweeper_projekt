using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Models
{
    public class Field : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
        #region Covered Property
        private bool covered = true;
        public bool Covered
        {
            get => covered;
            set { covered = value; NotifyPropertyChanged(); }
        }
        #endregion
        #region DangerLevel Property
        private int dangerLevel;
        public int DangerLevel
        {
            get => dangerLevel;
            set { dangerLevel = value; NotifyPropertyChanged(); }
        }
        #endregion
        #region IsMine Property
        public bool IsMine { get => DangerLevel == 10000; set { if (value) DangerLevel = 10000; } }
        #endregion
        public bool FirstClicked { get; set; } = false;
        public int Index { get; set; } = 0;
        #region IsSuspected Property
        private bool isSuspected = false;
        public bool IsSuspected 
        { 
            get => isSuspected;
            set { isSuspected = value; NotifyPropertyChanged(); }
        }
        #endregion
        public void IncreaseDangerLevel() => DangerLevel += 1;

        public void Reset()
        {
            Covered = true;
            FirstClicked = false;
            isSuspected = false;
            DangerLevel = 0;
        }
    }
}
