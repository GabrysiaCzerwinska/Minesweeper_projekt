using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Models
{
    public class Field : INotifyPropertyChanged
    {
        private bool _covered = true;
        private bool _isMine = false;
        private int _fieldValue = 0;
        private bool _isSuspected = false;






        public bool FirstClicked { get; set; } = false;
    

        public bool IsMine {
            get => _isMine;
            set
            {
                _isMine = value;
                DangerLevel = 10000; 
            }
        }
        public int DangerLevel 
        {
            get => _fieldValue;
            set
            {
                _fieldValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DangerLevel"));
            }
        }
        public int Index { get; set; } = 0;
        public bool Covered {
            get => _covered;
            set
            {
                _covered = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Covered"));
            }
        }

        public bool IsSuspected { get => _isSuspected;
            set
            {
                _isSuspected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSuspected"));
            }
        }

        public void increaseDangerLevel()
        {
            DangerLevel += 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
