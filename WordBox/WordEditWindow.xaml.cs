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
    /// WordEditWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WordEditWindow : Window
    {
        private WordGroup wordGroup;
        private int wordIndex;

        public WordEditWindow(WordGroup wordGroup, int wordIndex)
        {
            InitializeComponent();
            this.wordGroup = wordGroup;
            this.wordIndex = wordIndex;

            txtBoxLetter.Text = wordGroup[wordIndex].Letter;
            txtBoxMeaning.Text = wordGroup[wordIndex].Meaning;
            txtBoxMemo.Text = wordGroup[wordIndex].Memo;
            txtBoxWordState.Text = wordGroup[wordIndex].BoxAtr.BoxNumber.ToString();

            String[] timeString = wordGroup[wordIndex].BoxAtr.TimeString.Split('-');
            txtBoxYear.Text = timeString[0];
            txtBoxMonth.Text = timeString[1];
            txtBoxDay.Text = timeString[2];
            txtBoxHour.Text = timeString[3];
            txtBoxMinute.Text = timeString[4];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool hasNoError;
            try
            {
                String timeString = txtBoxYear.Text + "-" + txtBoxMonth.Text + "-" + txtBoxDay.Text + "-" + txtBoxHour.Text + "-" + txtBoxMinute.Text;

                hasNoError = this.wordGroup.EditWord(wordIndex, txtBoxLetter.Text, txtBoxMeaning.Text, txtBoxMemo.Text,
                    Int32.Parse(txtBoxWordState.Text), timeString );

                if (!hasNoError) throw new FormatException();

                this.Close();
            }
            catch(FormatException)
            {
                MessageBox.Show("오류:시간이나 상태에 적합한 숫자를 입력하여 주세요.", "오류");
            }
        }



        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            bool hasNoError;
            try
            {
                String timeString = "99-12-30-23-59";
                hasNoError = this.wordGroup.EditWord(wordIndex, txtBoxLetter.Text, txtBoxMeaning.Text, txtBoxMemo.Text, 100 , timeString);

                if (!hasNoError) throw new FormatException();
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("오류:시간이나 상태에 적합한 숫자를 입력하여 주세요.", "오류");
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
                    btnEnter.RaiseEvent(newEventArgs);
                    break;
                case Key.Delete:
                    btnDelete.RaiseEvent(newEventArgs);
                    break;
                case Key.Escape:
                    btnEsc.RaiseEvent(newEventArgs);
                    break;
            }
        }
    }
}
