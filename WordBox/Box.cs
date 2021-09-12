using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordBox
{
    public class Box
    {
        private int rightObjectBox;
        private bool isRightTimeRelatively;
        private String rightTimeString;
        private int wrongObjectBox;
        private bool isWrongTimeRelatively;
        private String wrongTimeString;

        public Box(int rightObjectBox, bool isRightTimeRelatively, String rightTimeString,
            int wrongObjectBox, bool isWrongTimeRelatively, String wrongTimeString)
        {
            this.rightObjectBox = rightObjectBox;
            this.isRightTimeRelatively = isRightTimeRelatively;
            this.rightTimeString = rightTimeString;
            this.wrongObjectBox = wrongObjectBox;
            this.isWrongTimeRelatively = isWrongTimeRelatively;
            this.wrongTimeString = wrongTimeString;
        }

        public int processBoxRule(bool isRightAnswer, BoxAttribute boxattribute)
        {
            if(isRightAnswer)
            {
                boxattribute.BoxNumber = this.rightObjectBox;
                boxattribute.TimeString = processTime(this.isRightTimeRelatively, boxattribute.TimeString, this.rightTimeString);
                return boxattribute.BoxNumber;
            }
            else
            {
                boxattribute.BoxNumber = this.wrongObjectBox;
                boxattribute.TimeString = processTime(this.isWrongTimeRelatively, boxattribute.TimeString, this.wrongTimeString);
                return boxattribute.BoxNumber;
            }
        }

        private String processTime(bool isRelatively, String wordTimeString, String boxTimeString)
        {

            DateTime dt;
            TimeSpan ts;
            String[] boxTimeSplit = boxTimeString.Split('/');
            int[] boxTimeSplitInt = new int[5];

            if (isRelatively)
            {
                dt = DateTime.Now;
                for(int i =2; i <= 4; i++)
                {
                    boxTimeSplitInt[i] = Convert.ToInt32(boxTimeSplit[i]);
                }
                ts = new TimeSpan(boxTimeSplitInt[2], boxTimeSplitInt[3], boxTimeSplitInt[4], 0);
                //0번 년도, 1번 월, 2번 일, 3번 시간, 4번은 분이며, TimeSpan은 Day까지의 단위만 표현 가능하기에 0,1을 사용안함.

                dt = dt.Add(ts);

                return dt.ToString("yy-MM-dd-HH-mm");
            }
            else
            {
                for(int i = 0; i <= 4; i++)
                {
                    boxTimeSplitInt[i] = Convert.ToInt32(boxTimeSplit[i]);
                }

                dt = new DateTime(boxTimeSplitInt[0], boxTimeSplitInt[1], boxTimeSplitInt[2], boxTimeSplitInt[3], boxTimeSplitInt[4], 0);

                return dt.ToString("yy-MM-dd-HH-mm");
            }
        }
    }
}
