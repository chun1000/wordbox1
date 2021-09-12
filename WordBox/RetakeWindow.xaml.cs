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
    /// RetakeWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RetakeWindow : Window
    {
        private WordGroup wordGroup;
        private bool memoSplashBack = false;
        private bool meaningSplashBack = false;
        private bool wordSplashBack = false;
        private bool getRetake = false;
        private bool hasNoRetakeWord = true;
        private List<ListViewMainItem> instance;
        public delegate void AnswerEventHandler(bool getRetake);
        public event AnswerEventHandler AnswerEvent;

        private class ListViewMainItem
        {
            public String Word { get; set; }
            public String Memo { get; set; }
            public String Meaning { get; set; }

            public ListViewMainItem(String word, String memo, String meaning)
            {
                Word = word;
                Memo = memo;
                Meaning = meaning;
            }
        }

        public RetakeWindow(WordGroup wordGroup, out bool hasNoRetakeWord)
        {
            InitializeComponent();
            this.wordGroup = wordGroup;
            RefreshListView();
            hasNoRetakeWord = this.hasNoRetakeWord;
        }

        private void RefreshListView()
        {
            String letter;
            String memo;
            String meaning;
            instance = new List<ListViewMainItem>();

            for (int i = 0; i < wordGroup.GetExamWordNum(); i++)
            {
                if (wordGroup.GetIsRetakeTarget(i))
                {
                    wordGroup.GetWordInfoForRetake(i, out letter, out meaning, out memo);
                    if (memoSplashBack) memo = "";
                    if (wordSplashBack) letter = "";
                    if (meaningSplashBack) meaning = "";

                    instance.Add(new ListViewMainItem(letter, memo, meaning));
                    hasNoRetakeWord = false;
                }
            }

            listViewMain.ItemsSource = instance;
            listViewMain.Items.Refresh();
        }

        private void btnWordSplashBack_Click(object sender, RoutedEventArgs e)
        {
            wordSplashBack = !wordSplashBack;
            RefreshListView();
        }

        private void btnMeaningSplashBack_Click(object sender, RoutedEventArgs e)
        {
            meaningSplashBack = !meaningSplashBack;
            RefreshListView();
        }

        private void btnMemoSplashBack_Click(object sender, RoutedEventArgs e)
        {
            memoSplashBack = !memoSplashBack;
            RefreshListView();
        }

        private void btnRetake_Click(object sender, RoutedEventArgs e)
        {
            getRetake = true;
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(AnswerEvent != null) AnswerEvent(getRetake);
        }

        //현재 리스트뷰의 선택된 단어의 TTS를 실행합니다.
        private void ExecuteTTS()
        {
            int index = listViewMain.SelectedIndex;
            Speech speech = new Speech();

            if(index >= 0)
            {
                speech.UseTTS(instance[index].Word);   
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Button.ClickEvent);

            switch (e.Key)
            {
                case Key.Enter:
                    btnRetake.RaiseEvent(newEventArgs);
                    break;
                case Key.F1:
                    btnWordSplashBack.RaiseEvent(newEventArgs);
                    break;
                case Key.F2:
                    btnMeaningSplashBack.RaiseEvent(newEventArgs);
                    break;
                case Key.F3:
                    btnMemoSplashBack.RaiseEvent(newEventArgs);
                    break;
                case Key.F4:
                    ExecuteTTS();
                    break;
                case Key.Escape:
                    btnEsc.RaiseEvent(newEventArgs);
                    break;
            }
        }

        private void btnEsc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
