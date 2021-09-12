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
using Microsoft.Win32;

namespace WordBox
{
    /// <summary>
    /// WordFileLoadWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WordFileLoadWindow : Window
    {
        private class PreviewItem
        {
            public String Word { get; set; }
            public String Meaning { get; set; }

            public PreviewItem(String word, String meaning)
            {
                this.Word = word;
                this.Meaning = meaning;
            }
        }

        private WordGroup wordGroup;
        private List<PreviewItem> instance;
        private static String prevProcessFile = "";
        private String currentFile;
        private String[] word;
        private String[] meaning;

        public WordFileLoadWindow(WordGroup wordgroup)
        {
            InitializeComponent();
            this.wordGroup = wordgroup;
        }

        private void btnDirectory_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                instance = new List<PreviewItem>();
                txtBoxDirectory.Text = openFileDialog.FileName;
                wordGroup.LoadWordInformationByTextFile(openFileDialog.FileName, out word, out meaning);
                for(int i =0; i < word.Length; i++)
                {
                    if (word[i] == null || meaning[i] == null) break;
                    instance.Add(new PreviewItem(word[i], meaning[i]));
                }
                listViewPreviewr.ItemsSource = instance;
                listViewPreviewr.Items.Refresh();
                currentFile = openFileDialog.FileName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            String str = "이전에 처리한 파일과 현재 파일이 동일한 이름을 가지고 있습니다. 그래도 이 단어들을 추가하시겠습니까?";
            if (prevProcessFile == currentFile)
            {
                if (MessageBox.Show(str, "경고", MessageBoxButton.YesNo) == MessageBoxResult.Yes) flag = true;
                else flag = false;
            }
            
            if(flag == true)
            {
                prevProcessFile = currentFile;
                for (int i = 0; i < word.Length; i++)
                {
                    if (word[i] == null || meaning[i] == null) break;
                    wordGroup.AddWord(word[i], meaning[i], "");
                }
            }

            Close();
        }
    }
}
