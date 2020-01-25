namespace Mineswepper
{
    class Field
    {
        public int dangerLevel;
        public bool isMined;
        public bool isSuspected;
        public bool isExposed;

        public Field()
        {
            dangerLevel = 0;
            isMined = false;
            isSuspected = false;
            isExposed = false;
        }

        public void increaseDangerLevel()
        {
            dangerLevel++;
        }

        public void mine()
        {
            isMined = true;
        }

        public void changeSuspect()
        {
            isSuspected = !isSuspected;
        }

        public void expose()
        {
            isExposed = true;
        }
    }
}