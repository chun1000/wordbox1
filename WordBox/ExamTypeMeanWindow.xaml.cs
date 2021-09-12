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
using System.Threading;

namespace WordBox
{
    /// <summary>
    /// ExamTypeMeanWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExamTypeMeanWindow : Window
    {
        public delegate void AnswerEventHandler(bool isRight);
        public event AnswerEventHandler AnswerEvent;
        private String rightAnswer;
        private bool isAutoScore;
        private bool isRightAns = false;

        public ExamTypeMeanWindow(bool isAutoScore, String rightAnswer, String letter, bool isRightAnswerBefore, bool isFirstWord)
        {
            InitializeComponent();
            this.rightAnswer = rightAnswer;
            this.isAutoScore = isAutoScore;
            txtBoxLetter.Text = letter;

            Thread thread = new Thread(() => { Speech speech = new Speech(); speech.UseTTS(letter); });
            thread.Start(); //Just Test

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

            txtBoxInput.Focus();
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (AnswerEvent != null)
            {
                if (isAutoScore)
                {
                    if (txtBoxInput.Text == rightAnswer) isRightAns = true;
                    else isRightAns = false;
                    this.Close();
                }
                else
                {
                    txtBoxRightAnswer.Text = this.rightAnswer;
                    btnYes.IsEnabled = true;
                    btnNo.IsEnabled = true;
                    txtBoxInput.IsReadOnly = true;
                    btnEnter.IsEnabled = false;
                }
            }
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            if (btnYes.IsEnabled)
            {
                isRightAns = true;
                this.Close();
            }
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            if (btnNo.IsEnabled)
            {
                isRightAns = false;
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
                case Key.Y:
                    btnYes.RaiseEvent(newEventArgs);
                    break;
                case Key.N:
                    btnNo.RaiseEvent(newEventArgs);
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AnswerEvent != null) AnswerEvent(isRightAns);
        }
    }
}
