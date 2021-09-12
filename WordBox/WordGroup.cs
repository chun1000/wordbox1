using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordBox
{
    public class WordGroup
    {
        private Word[] words;
        private FileManager fileManager;
        private BoxRule[] boxRules;
        private GroupInfo groupInfo;
        private int currentGroupNum;
        private int currentWordNum;
        private Exam exam;
        private Setting setting;
        private TempData tempData;
        //private Statistics statistics;


        public Word this[int index]
        {
            get
            {
                return words[index];
            }
            set
            {
                words[index] = value;
            }
        }

        public int CurrentWordNum { get { return currentWordNum; } }
        public int MAX_WORD_NUM { get; } = 50000;
        public int MAX_GROUP_NUM { get { return this.groupInfo.MAX_GROUP_NUM; } }
        public int MAX_BOXRULE_NUM { get; } = 30;
        public GroupInfo GroupInformation { get { return this.groupInfo; } }
        public Setting CurrentSetting { get { return this.setting; } set { this.setting = value; } }
        
        public int CurrentGroupNum { get { return this.currentGroupNum; } set { this.currentGroupNum = value; } }
        public String getBoxRuleName(int index )
        {
            return this.boxRules[index].RuleName;
        }

        public WordGroup()
        {

            fileManager = new FileManager();
            fileManager.LoadSetting(out setting);
            fileManager.LoadTempData(out tempData);
            fileManager.LoadGroupData(out groupInfo);

            currentGroupNum = tempData.CurrentGroupNum;
            fileManager.LoadWord(out words, MAX_WORD_NUM, currentGroupNum, setting.WordDirectory, ref currentWordNum);
            this.boxRules = new BoxRule[MAX_BOXRULE_NUM];
            for (int i = 0; i < MAX_BOXRULE_NUM; i++)
            {
                boxRules[i] = new BoxRule();
                boxRules[i].LoadBoxRule("Data/Rule", i);
            }
        
            
        }

        public WordGroup(int currentGroupNum)
        {

            fileManager = new FileManager();
            fileManager.LoadSetting(out setting);
            fileManager.LoadTempData(out tempData);
            fileManager.LoadGroupData(out groupInfo);

            this.currentGroupNum = currentGroupNum;
            tempData.CurrentGroupNum = currentGroupNum;

            fileManager.LoadWord(out words, MAX_WORD_NUM, currentGroupNum, setting.WordDirectory, ref currentWordNum);
            this.boxRules = new BoxRule[MAX_BOXRULE_NUM];
            for (int i = 0; i < MAX_BOXRULE_NUM; i++)
            {
                boxRules[i] = new BoxRule();
                boxRules[i].LoadBoxRule("Data/Rule", i);
            }


        }

        public bool saveDataForExit()
        {
            groupInfo.GroupWordNumHolding[currentGroupNum] = currentWordNum;
            fileManager.SaveSetting(setting);
            fileManager.SaveGroupData(groupInfo);
            
            fileManager.SaveWord(words, currentGroupNum, setting.WordDirectory, currentWordNum);
            fileManager.SaveTempData(tempData);//임시 변경, 이로 인해 버그가 생길수도 있음.
            System.Windows.Forms.MessageBox.Show(String.Format("{0}", tempData.FinishWordCounter));
            return true;
        }

        public bool SaveWordData(int currentGroup)
        {
            fileManager.SaveWord(words, currentGroup, setting.WordDirectory, currentWordNum);
            return true;
        }

        public bool AddWord(String word, String meaning, String memo)
        {
            if (currentWordNum < MAX_WORD_NUM)
            {
                DateTime dateTime = DateTime.Now;
                
                words[currentWordNum++] = new Word(word, meaning, 0, memo, 0, dateTime.ToString("yy-MM-dd-HH-mm"));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditWord(int wordIndex, String letter, String meaning, String memo, int wordState, String time)
        {
            bool timeError;

            if (wordIndex > MAX_WORD_NUM) return false;

            DateTime dt;
            timeError = DateTime.TryParseExact(time, "yy-MM-dd-HH-mm", null, System.Globalization.DateTimeStyles.None, out dt);
            if (timeError == false) return false;

            if ((wordState < 0) || (wordState > 100)) return false;

            words[wordIndex] = new Word(letter, meaning, words[wordIndex].WordState,memo, wordState, time);

            return true;
        }

        public void SelectWordForExam()
        {
            this.exam = new Exam(this.setting.WordNumPerExam);
            int count = 0;

            while(count < MAX_WORD_NUM && (words[count] != null))
            {
                if(words[count].IsSatisfiedTime && (words[count].BoxAtr.BoxNumber != 100))
                {
                    if (!exam.SetWordForExam(words[count])) break;
                }
                count++;
            }
        }

        public GroupInfo.ExamType GetWordExamType(int examIndex)
        {
            GroupInfo.ExamType currentGroupInfoExamType = this.groupInfo.GetExamType(examIndex);
            GroupInfo.ExamType wordExamType = GroupInfo.ExamType.Both;

            switch(currentGroupInfoExamType)
            {
                case GroupInfo.ExamType.Both:
                    wordExamType = exam.GetWordExamType(examIndex);
                    break;
                case GroupInfo.ExamType.LetterOnly:
                    wordExamType = GroupInfo.ExamType.LetterOnly;
                    break;
                case GroupInfo.ExamType.MeanOnly:
                    wordExamType = GroupInfo.ExamType.MeanOnly;
                    break;
                default:
                    break;
            }

            return wordExamType;


        }

        public void GetWordInfoForLetterExam(int examIndex, out String rightAnswer, out String hint, out String meaning)
        {
            exam.GetWordInfoForLetterExam(examIndex, this.setting, out rightAnswer, out hint, out meaning);
        }

        public void GetWordInfoForMeanExam(int examIndex, out String rightAnswer, out String letter)
        {
            exam.GetWordInfoForMeanExam(examIndex, out rightAnswer, out letter);
        }

        public void ProcessWordAfterExam(bool isRightAnswer, int examIndex)
        {
            exam.SetIsRetakeTarget(examIndex, !(isRightAnswer));
            if(isRightAnswer) exam.UpdateWordState(examIndex);
            BoxAttribute boxAttribute = this.exam.GetBoxRuleOfWord(examIndex);
            int boxRuleNum = this.GroupInformation.GroupRule[this.currentGroupNum];
            this.boxRules[boxRuleNum].ProcessBoxRule(isRightAnswer, boxAttribute);
        }

        public void ProcessWordAfterRetake(bool isRightAnswer, int examIndex)
        {
            exam.SetIsRetakeTarget(examIndex, !(isRightAnswer));
        }

        public bool GetWordInfoForRetake(int examIndex, out String letter, out String meaning, out String memo)
        {
            exam.GetWordInfoForRetake(examIndex, out letter, out meaning, out memo);
            return exam.GetIsRetakeTarget(examIndex);
        }

        public int GetExamWordNum()
        {
            return exam.WordNum;
        }

        public bool GetIsRetakeTarget(int examIndex)
        {
            return exam.GetIsRetakeTarget(examIndex);
        }

        public bool LoadWordInformationByTextFile(String address, out String[] letter, out String[] meaning)
        {
            return fileManager.LoadWordInformationByTextFile(address, out letter, out meaning);
        }
    }
}
