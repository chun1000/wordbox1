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
using System.Windows.Shapes;

namespace WordBox
{
    /// <summary>
    /// GroupWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GroupWindow : Window
    {


        private class ApplyingRuleListItem
        {
            private static List<ApplyingRuleListItem> instance = null;
            public String ApplyingRule { get; set; }

            public static List<ApplyingRuleListItem> getInstance()
            {
                if (instance == null) instance = new List<ApplyingRuleListItem>();
                return instance;
            }

            public ApplyingRuleListItem(String applyingRule)
            {
                this.ApplyingRule = applyingRule;
            }
        }
        
        private class GroupListItem
        {
            private static List<GroupListItem> instance = null;
            public String GroupName { get; set; }
            public String ApplyingRule { get; set; }

            public static List<GroupListItem> getInstance()
            {
                if (instance == null)
                {
                    instance = new List<GroupListItem>();
                }
                return instance;
            }

            public GroupListItem(String groupName, String applyingRule)
            {
                this.GroupName = groupName;
                this.ApplyingRule = applyingRule;
            }
        }

        WordGroup wordGroup;
        private bool isEditMode = false;
        private static bool isListNotLoaded = true;

        private void RefreshWindow(int index)
        {
            int currentRuleNumber = wordGroup.GroupInformation.GroupRule[index];

            listViewApplyingRule.SelectedIndex = currentRuleNumber;
            txtBoxApplyingRule.Text = wordGroup.getBoxRuleName(currentRuleNumber);
            txtBoxGroupName.Text = wordGroup.GroupInformation.GroupName[index];
            txtBoxWordNum.Text = wordGroup.GroupInformation.GroupWordNumHolding[index].ToString();

            TimeSpan ts =  wordGroup.GroupInformation.GroupFirstDelay[index];
            txtBoxDay.Text = ts.Days.ToString();
            txtboxMinute.Text = ts.Minutes.ToString();
            txtBoxHour.Text = ts.Hours.ToString();

            switch(wordGroup.GroupInformation.ExamTypes[index])
            {
                case GroupInfo.ExamType.LetterOnly:
                    comboExamMode.SelectedIndex = 0;
                    break;
                case GroupInfo.ExamType.MeanOnly:
                    comboExamMode.SelectedIndex = 1;
                    break;
                default:
                    comboExamMode.SelectedIndex = 2;
                    break;
            }
        }

        public GroupWindow(WordGroup wordGroup)
        {
            InitializeComponent();
            this.wordGroup = wordGroup;
            
            listViewGroup.ItemsSource = GroupListItem.getInstance();
            listViewApplyingRule.ItemsSource = ApplyingRuleListItem.getInstance();

            int i = 0;

            while((i < wordGroup.MAX_GROUP_NUM) && (wordGroup.GroupInformation.GroupName[i] != null)&&isListNotLoaded )
            {
                GroupListItem.getInstance().Add(new GroupListItem(wordGroup.GroupInformation.GroupName[i], wordGroup.getBoxRuleName(wordGroup.GroupInformation.GroupRule[i])));
                i++;
            }

            i = 0;

            while((i < wordGroup.MAX_BOXRULE_NUM) &&(wordGroup.getBoxRuleName(i) != null)&&isListNotLoaded)
            {
                ApplyingRuleListItem.getInstance().Add(new ApplyingRuleListItem(wordGroup.getBoxRuleName(i)));
                i++;
            }

            isListNotLoaded = false;

            listViewGroup.Items.Refresh();
            listViewGroup.SelectedIndex = wordGroup.CurrentGroupNum;

            listViewApplyingRule.Items.Refresh();
            RefreshWindow(wordGroup.CurrentGroupNum);
        }

        private void BtnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            
            int lastIndex = wordGroup.GroupInformation.LastIndex;

            if (lastIndex != -1)
            {
                wordGroup.GroupInformation.createDefaultGroup(lastIndex);
                GroupListItem.getInstance().Add(new GroupListItem(wordGroup.GroupInformation.GroupName[lastIndex], wordGroup.getBoxRuleName(wordGroup.GroupInformation.GroupRule[lastIndex])));
                listViewGroup.Items.Refresh();
            }
            else MessageBox.Show("그룹의 최대 생성 갯수를 초과하였습니다.", "오류");
        }

        private void btnDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = listViewGroup.SelectedIndex;
            String str;

            if (GroupListItem.getInstance().Count == 1)
            {
                MessageBox.Show("모든 그룹을 삭제할 수는 없습니다", "오류");
            }
            else if (index != -1)
            {
                str = "정말로 선택한 그룹(" + wordGroup.GroupInformation.GroupName[index] +")을 삭제하겠습니까? 해당 그룹의 모든 단어가 삭제됩니다.";

                if (MessageBox.Show(str, "알림", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    wordGroup.GroupInformation.deleteGroup(wordGroup.CurrentSetting, index);

                    

                    GroupListItem.getInstance().RemoveAt(index);
                    listViewGroup.Items.Refresh();
                }

            }
            else MessageBox.Show("선택된 그룹이 존재하지 않습니다.", "오류");
        }

        private void BtnEditGroup_Click(object sender, RoutedEventArgs e)
        {
            isEditMode = true;
            listViewApplyingRule.IsEnabled = true;
            txtBoxDay.IsReadOnly = false;
            txtBoxHour.IsReadOnly = false;
            txtboxMinute.IsReadOnly = false;
            txtBoxGroupName.IsReadOnly = false;
            listViewGroup.IsEnabled = false;
            btnCreateGroup.IsEnabled = false;
            btnDeleteBtn.IsEnabled = false;
            btnEditGroup.IsEnabled = false;
            btnEditApply.IsEnabled = true;
            btnEnter.IsEnabled = false;
            comboExamMode.IsEnabled = true;
        }

        private void BtnEditApply_Click(object sender, RoutedEventArgs e)
        {
            int[] temp = new int[3];

            try
            {
                temp[0] = Int32.Parse(txtBoxDay.Text);
                temp[1] = Int32.Parse(txtBoxHour.Text);
                temp[2] = Int32.Parse(txtboxMinute.Text);

                wordGroup.GroupInformation.SetGroupFirstDelayByInt(listViewGroup.SelectedIndex, temp);
                wordGroup.GroupInformation.GroupName[listViewGroup.SelectedIndex] = txtBoxGroupName.Text;
                wordGroup.GroupInformation.GroupRule[listViewGroup.SelectedIndex] = listViewApplyingRule.SelectedIndex;


                switch(comboExamMode.SelectedIndex)
                {
                    case 0:
                        wordGroup.GroupInformation.ExamTypes[listViewGroup.SelectedIndex] = GroupInfo.ExamType.LetterOnly;
                        break;
                    case 1:
                        wordGroup.GroupInformation.ExamTypes[listViewGroup.SelectedIndex] = GroupInfo.ExamType.MeanOnly;
                        break;
                    default:
                        wordGroup.GroupInformation.ExamTypes[listViewGroup.SelectedIndex] = GroupInfo.ExamType.Both;
                        break;
                }

                GroupListItem.getInstance()[listViewGroup.SelectedIndex].GroupName = txtBoxGroupName.Text;
                GroupListItem.getInstance()[listViewGroup.SelectedIndex].ApplyingRule = wordGroup.getBoxRuleName(listViewApplyingRule.SelectedIndex);
                listViewGroup.Items.Refresh();

                isEditMode = false;
                listViewApplyingRule.IsEnabled = false;
                txtBoxDay.IsReadOnly = true;
                txtBoxHour.IsReadOnly = true;
                txtboxMinute.IsReadOnly = true;
                txtBoxGroupName.IsReadOnly = true;
                listViewGroup.IsEnabled = true;
                btnCreateGroup.IsEnabled = true;
                btnDeleteBtn.IsEnabled = true;
                btnEditGroup.IsEnabled = true;
                btnEditApply.IsEnabled = false;
                btnEnter.IsEnabled = true;
                comboExamMode.IsEnabled = false;

                MessageBox.Show("편집이 완료되었습니다.", "알림");
            }
            catch (FormatException)
            {
                MessageBox.Show("시간은 숫자로 표현되어야 합니다.", "오류");
            }
            
            
        }

        private void ListViewGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewGroup.SelectedIndex == -1) listViewGroup.SelectedIndex = 0;
            RefreshWindow(listViewGroup.SelectedIndex);
        }

        private void ListViewApplyingRule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtBoxApplyingRule.Text = wordGroup.getBoxRuleName(listViewApplyingRule.SelectedIndex);
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            wordGroup.saveDataForExit();
            wordGroup.CurrentGroupNum = listViewGroup.SelectedIndex;
            Close();
        }

        private void BtnEsc_Click(object sender, RoutedEventArgs e)
        {
            if (isEditMode)
            {
                if(MessageBox.Show("변경 사항을 저장하지 않고 나가겠습니까?", "알림", MessageBoxButton.YesNo ) == MessageBoxResult.Yes)
                {
                    Close();
                }
            }
            else Close();
        }
    }
}
