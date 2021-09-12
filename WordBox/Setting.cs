using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordBox
{
    [Serializable]
    public class Setting
    {
        private int wordNumPerExam;
        private bool isAutoScore;
        private bool isRetake;
        private bool hasLetterNumHint;
        private bool hasFirstLetterHint;
        private bool hasLastLetterHint;
        private String backupDirectory;
        private String wordDirectory;

        public String WordDirectory { get { return this.wordDirectory; } }
        public int WordNumPerExam { get { return this.wordNumPerExam; } }
        public String BackupDirectory { get { return this.backupDirectory; } }

        public bool IsRetake {  get { return this.isRetake; } }
        public bool IsAutoScore { get { return this.isAutoScore; } }
        public bool HasLetterNumHint { get { return this.hasLetterNumHint; } }
        public bool HasFirstLetterHint { get { return this.hasFirstLetterHint; } }
        public bool HasLastLetterHint { get { return this.hasLastLetterHint; } }
        

        //디폴트 세팅 생성
        public Setting()
        {
            if (!Directory.Exists("Backup")) Directory.CreateDirectory("Backup");
            if (!Directory.Exists("Data/Word")) Directory.CreateDirectory("Data/Word");

            backupDirectory = "Backup";
            wordDirectory = "Data/Word";
            
            wordNumPerExam = 30;
            isAutoScore = false;
            isRetake = true;
            hasLetterNumHint = true;
            hasFirstLetterHint = false;
            hasLastLetterHint = false;

        }

        public Setting(int wordNumPerExam, bool isAutoScore, bool isRetake, bool hasLetterNumHint,
            bool hasFirstLetterHint, bool hasLastLetterHint, String backup, String word)
        {
            this.wordNumPerExam = wordNumPerExam;
            this.isAutoScore = isAutoScore;
            this.isRetake = isRetake;
            this.hasLetterNumHint = hasLetterNumHint;
            this.hasFirstLetterHint = hasFirstLetterHint;
            this.hasLastLetterHint = hasLastLetterHint;
            this.backupDirectory = backup;
            this.wordDirectory = word;
        }

        public String GetLetterExamHint(String input)
        {
            String output = "";

            if (this.hasLetterNumHint)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if ((i == 0) && (this.hasFirstLetterHint))
                    {
                        output += input[i];
                    }
                    else if((i == input.Length-1) && (this.hasLastLetterHint))
                    {
                        output += input[i];
                    }
                    else if(!char.IsLetterOrDigit(input[i]))
                    {
                        output += input[i];
                    }
                    else if(input[i] == ' ')
                    {
                        output += ' ';
                    }
                    else
                    {
                        output += "_";
                    }
                }
            }
            return output;
        }

    }
}
