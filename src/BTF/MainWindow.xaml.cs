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
using System.Windows.Threading;
using highlight;
using System.Diagnostics;
using Microsoft.Win32;

namespace BTF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath = "Example.bf";
        int lastLinenum = 0;
        const int memsize = 5000;
        int time = 0;
        InterPreter BrainFuck;
        System.Threading.CancellationTokenSource cts;
        delegate void Invoker();
        public DispatcherTimer Timer = new DispatcherTimer();
        new struct Tag
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }



        void Loade(object sender, RoutedEventArgs e)
        {
            Timer.Interval = TimeSpan.FromMilliseconds(1);
            Timer.Tick += new EventHandler(setTimerEvent);
            Timer.Start();
        }
        public MainWindow()
        {
            InitializeComponent();
            Paragraph p = CodeOutput.Document.Blocks.FirstBlock as Paragraph;
            p.LineHeight = 1;
            Paragraph o = CodeInput.Document.Blocks.FirstBlock as Paragraph;
            o.LineHeight = 1;
            Paragraph s = LineNumLabel.Document.Blocks.FirstBlock as Paragraph;
            s.LineHeight = 1;
            CodeInput.Document.PageWidth = 1000;
        }
        private static int GetLineNumber(RichTextBox rtb)
        {
            TextPointer tp1 = rtb.Selection.Start.GetLineStartPosition(0);
            TextPointer tp2 = rtb.Selection.Start;

            int column = tp1.GetOffsetToPosition(tp2);

            int someBigNumber = int.MaxValue;
            int lineMoved, currentLineNumber;
            rtb.Selection.Start.GetLineStartPosition(-someBigNumber, out lineMoved);
            currentLineNumber = -lineMoved;
            return currentLineNumber;
        }
        private void rectangle2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void closethis(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        
        static List<Tag> m_tags = new List<Tag>();
        static void CheckWordsInRun(Run run)
        {
            string text = run.Text;

            int sIndex = 0;
            int eIndex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsWhiteSpace(text[i]) | Highlighter.GetSpecials.Contains(text[i]))
                {
                    if (i > 0 && !(Char.IsWhiteSpace(text[i - 1]) | Highlighter.GetSpecials.Contains(text[i - 1])))
                    {
                        eIndex = i - 1;
                        string word = text.Substring(sIndex, eIndex - sIndex + 1);

                        if (Highlighter.IsKnownTag(word))
                        {
                            Tag t = new Tag();
                            t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                            t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                            t.Word = word;
                            m_tags.Add(t);
                        }
                    }
                    sIndex = i + 1;
                }
            }


            string lastWord = text.Substring(sIndex, text.Length - sIndex);
            if (Highlighter.IsKnownTag(lastWord))
            {
                Tag t = new Tag();
                t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                t.Word = lastWord;
                m_tags.Add(t);
            }
        }
     
        void Format()
        {
            CodeInput.TextChanged -= this.textChanged;

            for (int i = 0; i < m_tags.Count; i++)
            {
                TextRange range = new TextRange(m_tags[i].StartPosition, m_tags[i].EndPosition);
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.LightSkyBlue));
            }
            m_tags.Clear();

            CodeInput.TextChanged += this.textChanged;
        }
        void highlightEvent(RichTextBox textBox)
        {
            if (textBox.Document == null)
                return;

            TextRange documentRange = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);
            documentRange.ClearAllProperties();

            TextPointer navigator = textBox.Document.ContentStart;
            while (navigator.CompareTo(textBox.Document.ContentEnd) < 0)
            {
                TextPointerContext context = navigator.GetPointerContext(LogicalDirection.Backward);
                if (context == TextPointerContext.ElementStart && navigator.Parent is Run)
                {
                    CheckWordsInRun((Run)navigator.Parent);

                }
                navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
            }

            Format();
        }
        void NumLineEvent()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Invoker(NumLineEvent));
                return;
            }
            TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
            String[] Elines = textRange.Text.Split(new[] { Environment.NewLine }
                          , StringSplitOptions.RemoveEmptyEntries);
            if (Elines.Length != lastLinenum)
            {
                String[] lines = textRange.Text.Split(new[] { Environment.NewLine }
                       , StringSplitOptions.RemoveEmptyEntries);
                LineNumLabel.Document.Blocks.Clear();
                for (int line = 1; line < lines.Length + 1; line++)
                {
                    LineNumLabel.Document.Blocks.Add(new Paragraph(new Run(line.ToString())));
                }
                lastLinenum = lines.Length;
            }
        }
        private async void textChanged(object sender, TextChangedEventArgs e)
        {
            highlightEvent(this.CodeInput);
            await Task.Run(() => {
                NumLineEvent();
            });
    }
        private void setTimerEvent(object sender, EventArgs e)
        {
            TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
            Lineinfo.Content = $"줄:{GetLineNumber(CodeInput) + 1}   문자:{textRange.Text.Length - 2}   {filePath}";
        }
        private async void 번역하기()
        {
            TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
            if (textRange.Text != "")
            {
                if (comboBox.Text == "인간언어")
                {
                    BrainFuck = new HumanParser(textRange.Text, memsize);
                    번역.IsEnabled = false;
                    Stopwatch sw = new Stopwatch();
                    sw.Reset();
                    sw.Start();
                    await Task.Run(() =>
                    {
                        BrainFuck.RunCode();
                        ButtonEnable();
                    });
                    sw.Stop();
                    CodeOutput.Document.Blocks.Clear();
                    CodeOutput.Document.Blocks.Add(new Paragraph(new Run(BrainFuck.output)));
                    TextRange rangeOfText1 = new TextRange(CodeOutput.Document.ContentEnd, CodeOutput.Document.ContentEnd);
                    rangeOfText1.Text = "\nRun time:" + sw.ElapsedMilliseconds.ToString() + "ms";
                    rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.LightSkyBlue);
                    GC.Collect();
                }
                else if (comboBox.Text == "Ook!")
                {
                    번역.IsEnabled = false;
                    BrainFuck = new OokParser(textRange.Text, memsize);
                    await Task.Run(() =>
                    {
                        BrainFuck.RunCode();
                        ButtonEnable();
                    });
                    CodeOutput.Document.Blocks.Clear();
                    CodeOutput.Document.Blocks.Add(new Paragraph(new Run(BrainFuck.output)));
                    GC.Collect();
                }
                else if (comboBox.Text == "Javascript")
                {
                    번역.IsEnabled = false;
                    BrainFuck = new JsParser(textRange.Text, memsize);
                    await Task.Run(() =>
                    {
                        BrainFuck.RunCode();
                        ButtonEnable();
                    });
                    CodeOutput.Document.Blocks.Clear();
                    CodeOutput.Document.Blocks.Add(new Paragraph(new Run(BrainFuck.output)));
                    GC.Collect();
                    highlightEvent(this.CodeOutput);
                }
            }

        }
        private void 번역_Click(object sender, RoutedEventArgs e)
        {
            번역하기();
        }

        private void ButtonEnable()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Invoker(ButtonEnable));
                return;
            }
            번역.IsEnabled = true;
        }
        private void 번역_Copy_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(CodeOutput.Document.ContentStart, CodeOutput.Document.ContentEnd);
            Clipboard.SetText(textRange.Text);
        }

     private async void CodeInput_KeyDown(object sender, KeyEventArgs e)
        {


        }
        private void RichTextBox_ScrollC(object sender, ScrollChangedEventArgs e)
        {
            var textToSync = (sender == CodeInput) ? LineNumLabel : CodeInput;

            textToSync.ScrollToVerticalOffset(e.VerticalOffset);
            textToSync.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)//파일불러오기
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".btf";
            dlg.Filter = "BTF Files (*.btf)|";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(dlg.FileName))
                {
              string reading =await sr.ReadLineAsync();
                string filename = dlg.SafeFileName;
                filePath = filename;
                CodeInput.Document.Blocks.Add(new Paragraph(new Run(reading)));
            }
        }
    }
    }
}
