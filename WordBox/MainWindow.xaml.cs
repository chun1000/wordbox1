using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WordBox
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>

    public class MainListViewItem
    {
        public int Number { get; set; }
        public String Word { get; set; }
        public String Meaning { get; set; }
        public String Memo { get; set; }
        public int State { get; set; }
        public String Time { get; set; }
        public String SpecialProperty { get; set; } //NotYet(아직 등장안함, 회색), //Expired(삭제예정, 옅은 빨강)
        private static int count = 0;

        public static int Count { get { return count; } set { count = value; } }

        public MainListViewItem(Word word)
        {
            Number = count;
            count++;

            Word = word.Letter;
            Meaning = word.Meaning;
            Memo = word.Memo;
            State = word.BoxAtr.BoxNumber;
            Time = word.BoxAtr.TimeString;
            SpecialProperty = "";

            DateTime dt = DateTime.ParseExact(Time, "yy-MM-dd-HH-mm", null);
            if (DateTime.Compare(dt, DateTime.Now) >= 0)
            {
                SpecialProperty = "NotYet";
            }

            if (State == 100)
            {
                Memo = "곧 삭제가 예정 된 단어입니다.";
                State = 0;
                Time = "";
                SpecialProperty = "Expired";
            }
        }
    }

    public partial class MainWindow : Window
    {
        WordGroup wordGroup = null;
        private static List<MainListViewItem> instance;
        private bool isRightAnswer = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void GetStateFromChild(bool isRight)
        {
            this.isRightAnswer = isRight;
        }

        private void BtnExamStart_Click(object sender, RoutedEventArgs e)
        {
            wordGroup.SelectWordForExam();
            GroupInfo.ExamType examType;
            ExamTypeMeanWindow etmw;
            ExamTypeLetterWIndow etlw;
            gridMain.Visibility = Visibility.Hidden;
            String rightAnswer, hint, meaning, letter;
            bool isAutoScore = wordGroup.CurrentSetting.IsAutoScore;
            bool isFirstWord = true;
            bool hasNoRetakeWord;

            for (int i = 0; i < wordGroup.GetExamWordNum(); i++)
            {
                examType = wordGroup.GetWordExamType(i);
                examType = GroupInfo.ExamType.MeanOnly; //토익 시험을 위한 임시

                if (examType == GroupInfo.ExamType.LetterOnly)
                {
                    
                    wordGroup.GetWordInfoForLetterExam(i, out rightAnswer, out hint, out meaning);
                    etlw = new ExamTypeLetterWIndow(rightAnswer, hint, meaning, isRightAnswer, isFirstWord);
                    etlw.AnswerEvent += new ExamTypeLetterWIndow.AnswerEventHandler(GetStateFromChild);
                    etlw.Owner = this;
                    etlw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    etlw.ShowDialog();
                    etlw.AnswerEvent -= new ExamTypeLetterWIndow.AnswerEventHandler(GetStateFromChild);
                    wordGroup.ProcessWordAfterExam(this.isRightAnswer, i);
                }
                else
                {
                    wordGroup.GetWordInfoForMeanExam(i, out rightAnswer, out letter);
                    etmw = new ExamTypeMeanWindow(isAutoScore, rightAnswer, letter, isRightAnswer, isFirstWord);
                    etmw.AnswerEvent += new ExamTypeMeanWindow.AnswerEventHandler(GetStateFromChild);
                    etmw.Owner = this;
                    etmw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    etmw.ShowDialog();
                    etmw.AnswerEvent -= new ExamTypeMeanWindow.AnswerEventHandler(GetStateFromChild);
                    wordGroup.ProcessWordAfterExam(this.isRightAnswer, i);
                }

                isFirstWord = false;
            }

            if(wordGroup.CurrentSetting.IsRetake)
            {
                RetakeWindow rw = null;
                while (true)
                {
                    
                    rw = new RetakeWindow(wordGroup, out hasNoRetakeWord);

                    if (hasNoRetakeWord) break;

                    rw.AnswerEvent += new RetakeWindow.AnswerEventHandler(GetStateFromChild);
                    rw.Owner = this;
                    rw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    rw.ShowDialog();
                    rw.AnswerEvent -= new RetakeWindow.AnswerEventHandler(GetStateFromChild);

                    if (!isRightAnswer) break;

                    isFirstWord = true;

                    for (int i = 0; i < wordGroup.GetExamWordNum(); i++)
                    {


                        if (wordGroup.GetIsRetakeTarget(i))
                        {
                            examType = wordGroup.GetWordExamType(i);
                            examType = GroupInfo.ExamType.MeanOnly; //토익 시험을 위한 임시
                            if (examType == GroupInfo.ExamType.LetterOnly)
                            {
                                wordGroup.GetWordInfoForLetterExam(i, out rightAnswer, out hint, out meaning);
                                etlw = new ExamTypeLetterWIndow(rightAnswer, hint, meaning, isRightAnswer, isFirstWord);
                                etlw.AnswerEvent += new ExamTypeLetterWIndow.AnswerEventHandler(GetStateFromChild);
                                etlw.Owner = this;
                                etlw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                etlw.ShowDialog();
                                etlw.AnswerEvent -= new ExamTypeLetterWIndow.AnswerEventHandler(GetStateFromChild);
                                wordGroup.ProcessWordAfterRetake(isRightAnswer, i);
                            }
                            else
                            {
                                wordGroup.GetWordInfoForMeanExam(i, out rightAnswer, out letter);
                                etmw = new ExamTypeMeanWindow(isAutoScore, rightAnswer, letter, isRightAnswer, isFirstWord);
                                etmw.AnswerEvent += new ExamTypeMeanWindow.AnswerEventHandler(GetStateFromChild);
                                etmw.Owner = this;
                                etmw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                etmw.ShowDialog();
                                etmw.AnswerEvent -= new ExamTypeMeanWindow.AnswerEventHandler(GetStateFromChild);
                                wordGroup.ProcessWordAfterRetake(isRightAnswer, i);
                            }

                            isFirstWord = false;
                        }
                    }
                } 
            }

            gridMain.Visibility = Visibility.Visible;
            RefreshMainListView();
        }

        private void BtnGroupChange_Click(object sender, RoutedEventArgs e)
        {
            GroupWindow child = new GroupWindow(wordGroup);
            child.Owner = this;
            child.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            child.ShowDialog();
            wordGroup = new WordGroup(wordGroup.CurrentGroupNum);
            RefreshMainListView();
        }

        private void MitemGroupChange_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            wordGroup = new WordGroup();
           
            RefreshMainListView();
            

        }


        //메인 리스트뷰를 통째로 리프레시하는 함수입니다.
        private void RefreshMainListView()
        {
            instance = new List<MainListViewItem>();
            MainListViewItem.Count = 0;
            int currentGroupRuleNumber = wordGroup.GroupInformation.GroupRule[wordGroup.CurrentGroupNum];

            for (int i = 0; i < wordGroup.CurrentWordNum; i++)
            {
                instance.Add(new MainListViewItem(wordGroup[i]));
            }
            
            listViewMain.ItemsSource = instance;
            labelWordCount.Content = Convert.ToInt32(wordGroup.CurrentWordNum) + "/" + Convert.ToInt32(wordGroup.MAX_WORD_NUM);

            labelGroupName.Content = "그룹명: " + wordGroup.GroupInformation.GroupName[wordGroup.CurrentGroupNum] +
                "\t\t규칙명: " +wordGroup.getBoxRuleName(currentGroupRuleNumber);

            //리스트뷰의 스크롤을 맨 끝으로 내립니다.
            int count = listViewMain.Items.Count;
            if(count > 0)
            {
                object last = listViewMain.Items[count - 1];
                listViewMain.ScrollIntoView(last);
            }
            
        }
        

        private void TxtMeanAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter&&(txtWordAdd.Text != "")&&(txtMeanAdd.Text != ""))
            {
                wordGroup.AddWord(txtWordAdd.Text, txtMeanAdd.Text, txtMemoAdd.Text);
                txtWordAdd.Text = "";
                txtMeanAdd.Text = "";
                txtMemoAdd.Text = "";
                RefreshMainListView();
                txtWordAdd.Focus();
            }
            
        }

        private void TxtMemoAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (txtWordAdd.Text != "") && (txtMeanAdd.Text != ""))
            {
                wordGroup.AddWord(txtWordAdd.Text, txtMeanAdd.Text, txtMemoAdd.Text);
                txtWordAdd.Text = "";
                txtMeanAdd.Text = "";
                txtMemoAdd.Text = "";
                RefreshMainListView();
                txtWordAdd.Focus();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            wordGroup.saveDataForExit();
            Application.Current.Shutdown();
        }

        private void menuItemLoadWordFile_Click(object sender, RoutedEventArgs e)
        {
            WordFileLoadWindow wflw = new WordFileLoadWindow(this.wordGroup);
            wflw.ShowDialog();
            RefreshMainListView();
        }

        private void ListViewMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewMain.SelectedIndex != -1)
            {
                WordEditWindow wew = new WordEditWindow(wordGroup, listViewMain.SelectedIndex);
                wew.Owner = this;
                wew.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                wew.ShowDialog();
                RefreshMainListView();
            }
        }

        private void MenuItemBasicSetting_Click(object sender, RoutedEventArgs e)
        {
            BasicSettingWindow bsw = new BasicSettingWindow(wordGroup);
            bsw.ShowDialog();
        }
    }
}
