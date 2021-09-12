using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordBox
{
    public class BoxAttribute
    {
        private int boxNumber;
        private String timeString;

        public int BoxNumber { get { return boxNumber; } set { this.boxNumber = value; } }
        public String TimeString { get { return timeString; } set { this.timeString = value; } }

        public BoxAttribute(int boxNumber, String timeString)
        {
            this.boxNumber = boxNumber;
            this.timeString = timeString;
        }

        public override string ToString()
        {
            return boxNumber + "\t" + timeString;
        }
    }
}
