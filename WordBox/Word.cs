using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordBox
{
    public class Word
    {
        private String letter;
        private String meaning;
        private int wordState;
        private String memo;
        private BoxAttribute boxAttribute;

        public String Letter { get { return letter; } }
        public String Meaning { get { return meaning; } }
        public int WordState { get {return wordState; } set { wordState = value; } }
        public String Memo { get { return memo; } }
        public BoxAttribute BoxAtr { get { return boxAttribute; } }

        public bool IsSatisfiedTime
        {
            get
            {
                DateTime dt = DateTime.ParseExact(this.boxAttribute.TimeString, "yy-MM-dd-HH-mm", null);
                if (DateTime.Compare(dt, DateTime.Now) <= 0)
                {
                    return true;
                }
                else return false;
            }
        }


        public Word(String letter, String meaning, int wordState, String memo, int boxNumber, String timeString)
        {
            this.letter = letter;
            this.meaning = meaning;
            this.wordState = wordState;
            this.memo = memo;
            this.boxAttribute = new BoxAttribute(boxNumber, timeString);

        }

        public Word(String input)
        {
            int boxNumber; String timeString;
            String[] splitString = input.Split('\t');
            this.letter = splitString[0];
            this.meaning = splitString[1];
            this.wordState = Convert.ToInt32(splitString[2]);
            this.memo = splitString[3];
            boxNumber = Convert.ToInt32(splitString[4]);
            timeString = splitString[5];
            this.boxAttribute = new BoxAttribute(boxNumber, timeString);
        }

        public override string ToString()
        {
            String str = letter + "\t" + meaning + "\t" + wordState.ToString() + "\t" + memo + "\t" + boxAttribute.ToString();
            return str;
        }
    }
}
