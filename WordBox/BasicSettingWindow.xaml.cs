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
    /// BasicSettingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BasicSettingWindow : Window
    {
        private WordGroup wordGroup;

        public BasicSettingWindow(WordGroup wordGroup)
        {
            InitializeComponent();
            this.wordGroup = wordGroup;

            
            checkRetake.IsChecked = wordGroup.CurrentSetting.IsRetake;
            checkFirstLetterHint.IsChecked = wordGroup.CurrentSetting.HasFirstLetterHint;
            checkLastLetterHint.IsChecked = wordGroup.CurrentSetting.HasLastLetterHint;
            CheckLetterNumHint.IsChecked = wordGroup.CurrentSetting.HasLetterNumHint;
            txtBoxAddress.Text = wordGroup.CurrentSetting.WordDirectory;
            txtBoxWordPerExam.Text = wordGroup.CurrentSetting.WordNumPerExam.ToString();
        }

        private void BtnAddress_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtBoxAddress.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void BtnApplying_Click(object sender, RoutedEventArgs e)
        {
            int wordPerExam;

            try
            {
                wordPerExam = Int32.Parse(txtBoxWordPerExam.Text);
                if (wordPerExam < 1 || wordPerExam > 100) throw new FormatException();

                wordGroup.CurrentSetting = new Setting(wordPerExam, false, (bool)checkRetake.IsChecked, (bool)CheckLetterNumHint.IsChecked,
                    (bool)checkFirstLetterHint.IsChecked, (bool)checkLastLetterHint.IsChecked, "/Backup", txtBoxAddress.Text);

                Close();
            }
            catch(FormatException)
            {
                MessageBox.Show("한번에 시험 볼 단어의 개수는 (1-100)사이의 값이여야만 합니다.", "오류");
            }
        }

        private void BtnEsc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Button.ClickEvent);

            switch (e.Key)
            {
                case Key.Enter:
                    btnApplying.RaiseEvent(newEventArgs);
                    break;
                case Key.Escape:
                    btnEsc.RaiseEvent(newEventArgs);
                    break;
            }
        }

    }
}
