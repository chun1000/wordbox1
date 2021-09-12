using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordBox
{
    public class BoxRule
    {
        private String ruleName;
        private Box[] box = new Box[100];

        public String RuleName { get { return this.ruleName; } }

        public void makeDefaultRule()
        {

        }

        public bool hasBoxRule()
        {
            if (box[0] == null) return false;
            else return true;
        }

        public bool LoadBoxRule(String directory, int ruleNumber)
        {
            StreamReader sr = null;
            int curBoxNumber;
            int rightObjectBox;
            bool isRightTimeRelatively;
            String rightTimeString;
            int wrongObjectBox;
            bool isWrongTimeRelatively;
            String wrongTimeString;
            String[] buffer = new String[3];

            bool hasNoError = true;

            try
            {
                 sr = new StreamReader(directory + "/" + ruleNumber.ToString() + ".dat", Encoding.Default);
                this.ruleName = sr.ReadLine();

                while(!(sr.EndOfStream))
                {
                    curBoxNumber = Convert.ToInt32(sr.ReadLine());
                    buffer = sr.ReadLine().Split('\t');
                    rightObjectBox = Convert.ToInt32(buffer[0]);
                    if (buffer[1].Equals("R")) isRightTimeRelatively = true;
                    else isRightTimeRelatively = false;
                    rightTimeString = buffer[2];

                    buffer = sr.ReadLine().Split('\t');
                    wrongObjectBox = Convert.ToInt32(buffer[0]);
                    if (buffer[1].Equals("R")) isWrongTimeRelatively = true;
                    else isWrongTimeRelatively = false;
                    wrongTimeString = buffer[2];

                    this.box[curBoxNumber] = new Box(rightObjectBox, isRightTimeRelatively, rightTimeString
                        , wrongObjectBox, isWrongTimeRelatively, wrongTimeString);
                }

            }
            catch(Exception)
            {
                hasNoError = false;
            }
            finally
            {
                if(sr != null) sr.Close();
                
            }
            return hasNoError;

        }

        public void ProcessBoxRule(bool isRightAnswer, BoxAttribute boxAttribute)
        {
            int resultBoxNumber;
            if ( boxAttribute.BoxNumber != 100 && box[boxAttribute.BoxNumber] != null)
            {
                resultBoxNumber = this.box[boxAttribute.BoxNumber].processBoxRule(isRightAnswer, boxAttribute);

                if (box[resultBoxNumber] == null)
                {
                    boxAttribute.BoxNumber = 100;
                    boxAttribute.TimeString = "99-12-30-23-59";
                }
            }
            else
            {
                boxAttribute.BoxNumber = 100;
            }

           
        }
    }
}
