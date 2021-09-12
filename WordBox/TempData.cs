using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordBox
{
    [Serializable]
    public class TempData
    {
        private int currentGroupNum;
        private int finishWordCounter;

        public int CurrentGroupNum
        {
            get { return this.currentGroupNum; }
            set { this.currentGroupNum = value; }
        }

        public int FinishWordCounter
        {
            get { return this.finishWordCounter; }
            set { this.finishWordCounter = value; }
        }

        public TempData(int currentGroupNum)
        {
            this.currentGroupNum = currentGroupNum;
        }
    }
}
