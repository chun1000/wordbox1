using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace WordBox
{
    

    [Serializable]
    public class GroupInfo
    {

        public enum ExamType { LetterOnly, MeanOnly, Both }

        private int[] groupRule;
        private String[] groupName;
        private int[] groupWordNumHolding;
        private TimeSpan[] groupFirstDelay;
        private ExamType[] examType;

        public ExamType GetExamType(int index)
        {
            return this.examType[index];
        }

        public int LastIndex
        {
            get
            {
                int index = 0;
                while(index < groupName.Length)
                {
                    if (this.groupName[index] == null) return index;
                    index++;
                }
                return -1;
            }
        }

        public void createDefaultGroup(int index)
        {
            this.groupRule[index] = 0;
            this.groupName[index] = "이름없음";
            this.groupWordNumHolding[index] = 0;
            this.groupFirstDelay[index] = new TimeSpan(0);
            this.examType[index] = ExamType.Both;
        }

        public void createGroup(int index, int groupRule, String groupName, int[] dayHourMinute, ExamType examType)
        {
            this.groupRule[index] = groupRule;
            this.groupName[index] = groupName;
            this.groupWordNumHolding[index] = 0;
            this.SetGroupFirstDelayByInt(index, dayHourMinute);
            this.examType[index] = examType;
         }

        public void deleteGroup(Setting setting, int index)
        {
            String str = setting.WordDirectory;
            FileInfo file1, file2;
            while((index + 1 < groupName.Length) && (this.groupName[index+1] != null))
            {
                groupName[index] = groupName[index+1];

                file1 = new FileInfo(str + "/" + (index).ToString() + ".dat");
                file2 = new FileInfo(str + "/" + (index + 1).ToString() + ".dat");

                if (file1.Exists) file1.Delete();
                if (file2.Exists) file2.MoveTo(str + "/" + (index).ToString() + ".dat");
                index++;
            }
            
            groupName[index] = null;
            file1 = new FileInfo(str + "/" + (index).ToString() + ".dat");
            if (file1.Exists) file1.Delete();
        }

        public int[] GroupRule
        {
            get { return this.groupRule; }
            set { this.groupRule = value; }
        }

        public String[] GroupName
        {
            get { return this.groupName; }
            set { this.groupName = value; }
        }

        public int[] GroupWordNumHolding
        {
            get { return this.groupWordNumHolding; }
            set { this.groupWordNumHolding = value; }
        }

        public TimeSpan[] GroupFirstDelay
        {
            get { return this.groupFirstDelay; }
            set { this.groupFirstDelay = value; }
        }

        public ExamType[] ExamTypes
        {
            get { return this.examType; }
            set { this.ExamTypes = value; }
        }


        public int MAX_GROUP_NUM { get; } = 30;

        public GroupInfo()
        {
            this.groupRule = new int[MAX_GROUP_NUM];
            this.groupName = new String[MAX_GROUP_NUM];
            this.groupWordNumHolding = new int[MAX_GROUP_NUM];
            this.groupFirstDelay = new TimeSpan[MAX_GROUP_NUM];
            this.examType = new ExamType[MAX_GROUP_NUM];
        }

        public int[] GetGroupFirstDelayByInt(int index)
        {
            int[] returnInt = new int[3];

            returnInt[0] = this.GroupFirstDelay[index].Days;
            returnInt[1] = this.GroupFirstDelay[index].Hours;
            returnInt[2] = this.GroupFirstDelay[index].Minutes;

            return returnInt;
        }

        public void SetGroupFirstDelayByInt(int index, int[] dayHourMinute)
        {
            this.GroupFirstDelay[index] = new TimeSpan(dayHourMinute[0], dayHourMinute[1], dayHourMinute[2], 0);
        }
    }
}
