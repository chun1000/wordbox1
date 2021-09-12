using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WordBox
{
    public class FileManager
    {
        private String settingAddress = "Setting/Setting.dat";
        private int counter;

        //save, load 계열 함수들은 정상적인 저장 상황에서 true를 아니면 false를 반환함.
        public bool SaveSetting(Setting setting)
        {
            Stream fs = null;
            bool errorFlag = true;

            try
            {
                fs = new FileStream(settingAddress, FileMode.Create);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fs, setting);

            }
            catch (Exception)
            {
                errorFlag = false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }

            return errorFlag;
        }

        public bool LoadSetting(out Setting setting)
        {
            if (!Directory.Exists("Setting")) Directory.CreateDirectory("Setting");
            Stream fs = null;
            try
            {
                fs = new FileStream(settingAddress, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                setting = binaryFormatter.Deserialize(fs) as Setting;
                fs.Close();
                return true;
            }
            catch(FileNotFoundException)
            {
                if (fs != null) fs.Close();
                fs = new FileStream(settingAddress, FileMode.Create);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                setting = new Setting();
                binaryFormatter.Serialize(fs, setting);
                fs.Close();
            }

            return false;
      
        }

        public bool SaveTempData(TempData tempData)
        {
            Stream fs = null;
            bool errorFlag = true;
            tempData.FinishWordCounter = counter;

            try
            {
                fs = new FileStream("Data/Temp.dat", FileMode.Create);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fs, tempData);
                
            }
            catch(Exception)
            {
                errorFlag = false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }

            return errorFlag;
        }

        public bool LoadTempData(out TempData tempData)
        {
            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");
            Stream fs = null;

            try
            {
                fs = new FileStream("Data/Temp.dat", FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                tempData = binaryFormatter.Deserialize(fs) as TempData;
                fs.Close();
                counter = tempData.FinishWordCounter;
                return true;
            }
            catch (Exception)
            {
                if (fs != null) fs.Close();
                fs = new FileStream("Data/Temp.dat", FileMode.Create);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                tempData = new TempData(0);
                binaryFormatter.Serialize(fs, tempData);
                fs.Close();
            }

            return false;
        }

        public bool SaveWord(Word[] words, int currentGroupNum, String wordDirectory, int wordNum)
        {
            bool errorFlag = true;
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(wordDirectory + "/" + currentGroupNum.ToString() + ".dat", false, Encoding.Default);
                for (int i = 0; i < wordNum; i++)
                {
                    if (words[i].BoxAtr.BoxNumber != 100) sw.WriteLine(words[i].ToString());
                    else counter++;
                }
            }
            catch(Exception)
            {
                errorFlag = false;
            }
            finally
            {
                if (sw != null) sw.Close();
            }

            return errorFlag;
        }
        
        public bool LoadWord(out Word[] words, int maxWordNum, int currentGroupNum, String wordDirectory, ref int count)
        {
            words = new Word[maxWordNum];
            String buffer;
            if (!Directory.Exists(wordDirectory)) Directory.CreateDirectory(wordDirectory);
            StreamReader sr = null; StreamWriter sw = null;

            try
            {
                sr = new StreamReader(wordDirectory + "/" + currentGroupNum.ToString() + ".dat", Encoding.Default);

                while((!sr.EndOfStream)&& count < maxWordNum)
                {
                    buffer = sr.ReadLine();
                    words[count] = new Word(buffer);
                    count++;
                }
  
            }
            catch(FileNotFoundException)
            {
                if (sr != null) sr.Close();
                sw = new StreamWriter(wordDirectory + "/" + currentGroupNum.ToString() + ".dat", false, Encoding.Default);
                words[0] = new Word("Default", "더블클릭으로 삭제해주세요", 0, "메모없음", 0, "00-01-01-00-00");
                sw.WriteLine(words[0].ToString());
            }
            finally
            {
                if (sr != null) sr.Close();
                if (sw != null) sw.Close();
            }

            return false;
        }

        public bool SaveGroupData(GroupInfo groupInfo)
        {
            Stream fs = null;
            bool errorFlag = true;

            try
            {
                fs = new FileStream("Data/Group.dat", FileMode.Create);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fs, groupInfo);

            }
            catch (Exception)
            {
                errorFlag = false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }

            return errorFlag;
        }
        
        public bool LoadGroupData(out GroupInfo groupinfo)
        {
            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");
            if (!Directory.Exists("Data/Word")) Directory.CreateDirectory("Data/Word");
            Stream fs = null;

            try
            {
                fs = new FileStream("Data/Group.dat", FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                groupinfo = binaryFormatter.Deserialize(fs) as GroupInfo;
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                if (fs != null) fs.Close();
                fs = new FileStream("Data/Group.dat", FileMode.Create);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                groupinfo = new GroupInfo();

                groupinfo.GroupName[0] = "기본 그룹";
                groupinfo.GroupRule[0] = 0;
                groupinfo.GroupWordNumHolding[0] = 0;
                groupinfo.GroupFirstDelay[0] = new TimeSpan(0);

                binaryFormatter.Serialize(fs, groupinfo);
                
                fs.Close();
            }

            return false;
        }

        public bool LoadWordInformationByTextFile(String address, out String[] letter, out String[] mean)
        {
            letter = new String[10000];
            mean = new String[10000];
            bool hasNoError = true;
            String[] buffer;
            int count = 0;
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(address, Encoding.Default);
                while (!sr.EndOfStream)
                {
                    buffer = sr.ReadLine().Split('\t');
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        if(buffer[i] != "")
                        {
                            if(count %2 == 0)
                            {
                                letter[count / 2] = buffer[i];
                            }
                            else
                            {
                                mean[count / 2] = buffer[i];
                            }
                            count++;
                        }
                    }
                }


            }
            catch(Exception)
            {
                hasNoError = false;
            }
            finally
            {
                sr.Close();
            }
            return hasNoError;
        }
    }
}
