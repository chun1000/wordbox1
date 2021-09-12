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
    /// ExamTypeLetterWIndow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExamTypeLetterWIndow : Window
    {
        public delegate void AnswerEventHandler(bool isRight);
        public event AnswerEventHandler AnswerEvent;
        private String rightAnswer;
        private bool isRightAns = false;

        public ExamTypeLetterWIndow(String rightAnswer, String hint, String meaning, bool isRightAnswerBefore, bool isFirstWord)
        {
            InitializeComponent();
            this.rightAnswer = rightAnswer;
            txtBoxInput.Focus();

            txtBoxHint.Text = hint;
            txtBoxMeaning.Text = meaning;

            if (isFirstWord == false)
            {

                if (isRightAnswerBefore)
                {
                    labelPreviousResult.Content = "맞았습니다!";
                    labelPreviousResult.Foreground = Brushes.Green;
                }
                else
                {
                    labelPreviousResult.Content = "틀렸습니다!";
                    labelPreviousResult.Foreground = Brushes.Red;
                }

            }



        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (AnswerEvent != null)
            {
                if (txtBoxInput.Text == rightAnswer) isRightAns = true;
                else isRightAns = false;
                this.Close();
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Button.ClickEvent);

            switch (e.Key)
            {
                case Key.Enter:
                    btnEnter.RaiseEvent(newEventArgs);
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AnswerEvent != null) AnswerEvent(isRightAns);
        }
    }
}
