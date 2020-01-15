using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mineswepper {
    class Field {
        public int dangerLevel;
        public bool isBombed;
        public bool isSuspected;
        public bool isExposed;

        public Field() {
            dangerLevel = 0;
            isBombed = false;
            isSuspected = false;
            isExposed = false;
        }

        public void increaseDangerLevel() {
            dangerLevel++;
        }

        public void bomb() {
            isBombed = true;
        }

        public void suspect() {
            isSuspected = true;
        }

        public void expose() {
            isExposed = true;
        }
    }
}