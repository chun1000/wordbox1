using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordBox
{
    public class Exam
    {
        private Word[] words;
        private int wordNum;
        private int maxWordNum;
        private bool[] isRetakeTarget;

        public bool GetIsRetakeTarget(int index)
        {
            return isRetakeTarget[index];
        }
        public void SetIsRetakeTarget(int index, bool value)
        {
            isRetakeTarget[index] = value;
        }

        public int WordNum { get { return this.wordNum; } }

        public Exam(int num)
        {
            maxWordNum = num;
            wordNum = 0;
            this.words = new Word[num];
            this.isRetakeTarget = new bool[num];
        }

        public bool SetWordForExam(Word word)
        {
            if(wordNum < maxWordNum)
            {
                isRetakeTarget[wordNum] = false;
                words[wordNum++] = word;
                return true;
            }
            else
            {
                return false;
            }
        }

        public GroupInfo.ExamType GetWordExamType(int index)
        {
            int examDef = words[index].WordState % 2;
            GroupInfo.ExamType wordExamType;

            switch(examDef)
            {
                case 0:
                    wordExamType = GroupInfo.ExamType.LetterOnly;
                    break;
                case 1:
                    wordExamType = GroupInfo.ExamType.MeanOnly;
                    break;
                default:
                    wordExamType = GroupInfo.ExamType.LetterOnly;
                    break;
            }

            return wordExamType;
        }

        public void GetWordInfoForLetterExam(int examIndex, Setting setting, out String rightAnswer, out String hint, out String meaning)
        {
            rightAnswer = words[examIndex].Letter;
            meaning = words[examIndex].Meaning;
            hint = setting.GetLetterExamHint(words[examIndex].Letter);
        }

        public void GetWordInfoForMeanExam(int examIndex, out String rightAnswer, out String letter)
        {
            rightAnswer = words[examIndex].Meaning;
            letter = words[examIndex].Letter;
        }

        public void GetWordInfoForRetake(int examIndex, out String letter, out String meaning, out String memo)
        {
            letter = words[examIndex].Letter;
            meaning = words[examIndex].Meaning;
            memo = words[examIndex].Memo;
        }

        public void UpdateWordState(int examIndex)
        {
            words[examIndex].WordState += 1;
        }

        public BoxAttribute GetBoxRuleOfWord(int examIndex)
        {
            return this.words[examIndex].BoxAtr;
        }
    }
}
