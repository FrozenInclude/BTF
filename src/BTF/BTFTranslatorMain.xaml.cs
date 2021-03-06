﻿using System;
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
using System.Threading;
using System.IO;
using System.Windows.Threading;
using highlight;
using System.Diagnostics;
using Microsoft.Win32;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;

namespace BTF
{
    public partial class BTFTranslator : Window
    {
        static private Queue<string> FilePathQue = new Queue<string>();
        private FileSystem file;
        private const string DatainiPath = "save.ini";
        private string lastFileText = "";
        private string filePath = "Example.bf";
        private int lastLinenum = 0;
        private const int memsize = 5000;
        private InterPreter BrainFuck;
        private bool optionshow = false;
        private delegate void Invoker();
        private delegate void HilightDelegate(RichTextBox codeinput, bool isBF);
        private readonly SynchronizationContext synchronizationContext;//비동기처리용
        private DateTime previousTime = DateTime.Now;//비동기처리용
        private DispatcherTimer Timer = new DispatcherTimer();
        private DispatcherTimer OutputDisplayTimer = new DispatcherTimer(DispatcherPriority.Background);//비동기처리
        private new struct Tag
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }
        private async void checkStart()//파일연결
        {
            if (Environment.GetCommandLineArgs().Length == 2)
            {
                if (Environment.GetCommandLineArgs()[1] != null)
                {
                    filePath = Environment.GetCommandLineArgs()[1];
                    System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
                    string reading = await sr.ReadLineAsync();
                    string filename = filePath;
                    SetTextBoxText(CodeInput, reading);
                }
            }
        }
        private void saveCheckBoxData()
        {
            iniSystem a = new iniSystem(DatainiPath);
            a.Write("DynamicMemory", DynamicList.IsChecked.Value.ToString(), "CheckBox");
            a.Write("RunningTime", runningTime.IsChecked.Value.ToString(), "CheckBox");
            a.Write("memoryView", memoryView.IsChecked.Value.ToString(), "CheckBox");
            a.Write("countingView", CoutingPointer.IsChecked.Value.ToString(), "CheckBox");
            a.Write("8bit", bit8.IsChecked.Value.ToString(), "CheckBox");
            a.Write("16bit", bit16.IsChecked.Value.ToString(), "CheckBox");
            a.Write("32bit", bit32.IsChecked.Value.ToString(), "CheckBox");
        }
        private void loadCheckBoxData()
        {
            iniSystem a = new iniSystem(DatainiPath);
            try {
                DynamicList.IsChecked = Convert.ToBoolean(a.Read("DynamicMemory", "CheckBox"));
                runningTime.IsChecked = Convert.ToBoolean(a.Read("RunningTime", "CheckBox"));
                memoryView.IsChecked = Convert.ToBoolean(a.Read("memoryView", "CheckBox"));
                CoutingPointer.IsChecked = Convert.ToBoolean(a.Read("countingView", "CheckBox"));
                bit8.IsChecked = Convert.ToBoolean(a.Read("8bit", "CheckBox"));
                bit16.IsChecked = Convert.ToBoolean(a.Read("16bit", "CheckBox"));
                bit32.IsChecked = Convert.ToBoolean(a.Read("32bit", "CheckBox"));
            }
            catch (Exception)
            {
                bit8.IsChecked = true;
                return;
            }
        }
        private void SaveCheck(bool tryExit)
        {
            TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
            if (textRange.Text != lastFileText)
            {
                MessageBoxResult result = MessageBox.Show("저장하시겠습니까?", "알림", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                    if (tryExit)
                    {
                        saveCheckBoxData();
                        file.SaveQueue();
                        Environment.Exit(0);
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    if (tryExit)
                    {
                        saveCheckBoxData();
                        file.SaveQueue();
                        Environment.Exit(0);
                    }
                    return;
                }
                else if (result == MessageBoxResult.None)
                {
                    return;
                }
            }
            else
            {
                if (tryExit)
                {
                    saveCheckBoxData();
                    file.SaveQueue();
                    Environment.Exit(0);
                }
            }
        }
        private void Loade(object sender, RoutedEventArgs e)
        {
            mediaElement1.Source = new Uri(Directory.GetCurrentDirectory() + @"\sound\complete.mp3");
            Timer.Interval = TimeSpan.FromMilliseconds(1);
            Timer.Tick += new EventHandler(setTimerEvent);
            Timer.Start();
            OutputDisplayTimer.Interval = TimeSpan.FromMilliseconds(50);
            OutputDisplayTimer.Tick += new EventHandler(DisPlayOutputTimerEvent);
            OutputDisplayTimer.Start();
            checkStart();
            loadCheckBoxData();
        }
        public BTFTranslator()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            Paragraph p = CodeOutput.Document.Blocks.FirstBlock as Paragraph;
            p.LineHeight = 1;
            Paragraph o = CodeInput.Document.Blocks.FirstBlock as Paragraph;
            o.LineHeight = 1;
            Paragraph s = LineNumLabel.Document.Blocks.FirstBlock as Paragraph;
            s.LineHeight = 1;
            CodeInput.Document.PageWidth = 1000;
            CodeOutput.Document.PageWidth = 5000;
            recentFile.Items.Clear();
            file = new FileSystem(ref FilePathQue, ref recentFile, DatainiPath);
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
            return currentLineNumber + 1;
        }
       
        private static int GetLineLength(string s)
        {
            int count = 0;
            int position = 0;
            while ((position = s.IndexOf('\n', position)) != -1)
            {
                count++;
                position++;      
            }
            return count;
        }
        private void rectangle2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try {
                this.DragMove();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void closethis(object sender, RoutedEventArgs e)
        {
            SaveCheck(true);
        }

        public static void DisplayQue(MenuItem menu)
        {
            menu.Items.Clear();
            foreach (string item in FilePathQue)
            {
                menu.Items.Add(item);
            }
        }

        private List<Tag> m_tags = new List<Tag>();
        private void CheckWordsInRun(Run run)
        {
            string text = run.Text;

            int sIndex = 0;
            int eIndex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsWhiteSpace(text[i]) | Highlighter.GetSpecial.Contains(text[i]))
                {
                    if (i > 0 && !(Char.IsWhiteSpace(text[i - 1]) | Highlighter.GetSpecial.Contains(text[i - 1])))
                    {
                        eIndex = i - 1;
                        string word = text.Substring(sIndex, eIndex - sIndex + 1);

                        if (Highlighter.IsKnown(word))
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
            if (Highlighter.IsKnown(lastWord))
            {
                Tag t = new Tag();
                t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                t.Word = lastWord;
                m_tags.Add(t);
            }
        
    }
  
        private void Format()
        {
            if (!CodeOutput.Dispatcher.CheckAccess())
            {
                CodeOutput.Dispatcher.BeginInvoke(new Action(() => { Format(); }));
               // Dispatcher.BeginInvoke(new Invoker(Format));
                return;
            }
            for (int i = 0; i < m_tags.Count; i++)
            {
                TextRange range = new TextRange(m_tags[i].StartPosition, m_tags[i].EndPosition);
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Aqua));
            }
            m_tags.Clear();
        }
        private void highlightEvent(RichTextBox textBox)
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
         //   Format(Color);
        }

        private void NumLineEvent()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Invoker(NumLineEvent));
                return;
            }
            TextRange text = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);

            int linelength = GetLineLength(text.Text);
            //MessageBox.Show(linelength.ToString());
            if (!LineNumLabel.Dispatcher.CheckAccess())
            {
                LineNum.Dispatcher.BeginInvoke(new Action(() => { NumLineEvent(); }));
                // LineNumLabel.Document.Blocks.Clear();
            }
            if (GetLineLength(text.Text) != lastLinenum)
            {
                LineNumLabel.Document.Blocks.Clear();
                for (int line = 1; line < linelength + 1; line++)
                {
                    LineNumLabel.Document.Blocks.Add(new Paragraph(new Run(line.ToString())));
                }
                lastLinenum = GetLineLength(text.Text);
            }
        }

        private void SetTextBoxText(RichTextBox textbox, string appendText)
        {
           textBox.Focus();
            textbox.Document.Blocks.Clear();
            textbox.Document.Blocks.Add(new Paragraph(new Run(appendText)));
        }
        private String GetFullPathWithoutExtension(String path)
        {
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileNameWithoutExtension(path));
        }
        private void outputExE(string Csharpcode, string outputpath, ref string output)
        {
            CodeDomProvider codeDom = CodeDomProvider.CreateProvider("CSharp");
   
            // 컴파일러 파라미터 옵션 지정
            CompilerParameters cparams = new CompilerParameters();
            cparams.GenerateExecutable = true;

            cparams.OutputAssembly = outputpath + ".exe";

            // 소스코드를 컴파일해서 EXE 생성
            CompilerResults results = codeDom.CompileAssemblyFromSource(cparams, Csharpcode);

            // 컴파일 에러 있는 경우 표시
            if (results.Errors.Count > 0)
            {
                foreach (var err in results.Errors)
                {
                    Console.WriteLine(err.ToString());
                }
                return;
            }
            string filepaths = GetFullPathWithoutExtension(filePath);
            File.Copy(outputpath + ".exe", filepaths + ".exe", true);
            File.Delete(outputpath + ".exe");
            if (File.Exists(filepaths + ".exe"))
            {
                Process.Start(filepaths + ".exe");
            }
        }
        private async void textChanged(object sender, TextChangedEventArgs e)
        {
            await Task.Run(() =>
            {//하이라이팅이벤트 비동기처리 
                NumLineEvent();
            });

        }
        private void DisPlayOutputTimerEvent(object sender, EventArgs e)//줄및문자수 세기용
        {
          if (BrainFuck != null)
            {
                if (!번역.IsEnabled&&comboBox.Text == "인간언어")
                {
                        if (BrainFuck.output != "")
                        {
                            SetTextBoxText(CodeOutput, BrainFuck.output);
                            CodeOutput.ScrollToEnd();
                        }
                }
            }
        }
        private void setTimerEvent(object sender, EventArgs e)//줄및문자수 세기용
        {
            if (DynamicList.IsChecked == true)
            {
                textBox.IsEnabled = false;
            } else
            {
                textBox.IsEnabled = true;
            }
            TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
            Lineinfo.Content = $"줄:{GetLineNumber(CodeInput)}   문자:{textRange.Text.Length - 2}   {System.IO.Path.GetFileName(filePath)}";
            if (설정.IsMouseOver)
                rot.Angle += 2;
            else
                rot.Angle = 0;
        }
           private async Task 번역하기()
             {

                 if (comboBox.Text != "")
                 {
                     TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
                     if (textRange.Text != "")
                     {
                         SetTextBoxText(CodeOutput, "번역중...");
                         //await Task.Delay(250);
                         if (comboBox.Text == "인간언어")
                         {
                             try {
                                 if (bit8.IsChecked == true)
                                 {
                                     if (DynamicList.IsChecked == false)
                                         BrainFuck = new HumanParser(textRange.Text, InterPreter.CellSize.bit8, CoutingPointer.IsChecked, int.Parse(textBox.Text), memoryView.IsChecked.Value, false, InputBox.Text);
                                     else if (DynamicList.IsChecked == true)
                                         BrainFuck = new HumanParser(textRange.Text, InterPreter.CellSize.bit8, CoutingPointer.IsChecked, 0, memoryView.IsChecked.Value, false, InputBox.Text);
                                 }
                                 else if (bit16.IsChecked == true)
                                 {
                                     if (DynamicList.IsChecked == false)
                                         BrainFuck = new HumanParser(textRange.Text, InterPreter.CellSize.bit16, CoutingPointer.IsChecked, int.Parse(textBox.Text), memoryView.IsChecked.Value, false, InputBox.Text);
                                     else if (DynamicList.IsChecked == true)
                                         BrainFuck = new HumanParser(textRange.Text, InterPreter.CellSize.bit16, CoutingPointer.IsChecked, 0, memoryView.IsChecked.Value, false, InputBox.Text);
                                 }
                                 else if (bit32.IsChecked == true)
                                 {
                                     if (DynamicList.IsChecked == false)
                                         BrainFuck = new HumanParser(textRange.Text, InterPreter.CellSize.bit32, CoutingPointer.IsChecked, int.Parse(textBox.Text), memoryView.IsChecked.Value, false, InputBox.Text);
                                     else if (DynamicList.IsChecked == true)
                                         BrainFuck = new HumanParser(textRange.Text, InterPreter.CellSize.bit32, CoutingPointer.IsChecked, 0, memoryView.IsChecked.Value, false, InputBox.Text);
                                 }
                             }
                             catch (Exception)
                             {
                                 SetTextBoxText(CodeOutput, "포인터 메모리 크기 설정이 잘못되었습니다.");
                                 return;
                             }
                             번역.IsEnabled = false;
                             중단.IsEnabled = true;
                             Stopwatch sw = new Stopwatch();
                             sw.Reset();
                             sw.Start();
                             await Task.Run(() =>
                             {
                                 BrainFuck.RunCode();
                                 ButtonEnable();
                             });
                             sw.Stop();
                             SetTextBoxText(CodeOutput, BrainFuck.output);
                             if (runningTime.IsChecked == true)
                             {
                                 TextRange rangeOfText1 = new TextRange(CodeOutput.Document.ContentEnd, CodeOutput.Document.ContentEnd);
                                 rangeOfText1.Text = "\nRun time:" + sw.ElapsedMilliseconds.ToString() + "ms";
                                rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.LightSkyBlue);
                             }
                             GC.Collect();

                         }
                         else if (comboBox.Text == "인간언어(EXE)")
                         {
                             if (filePath != "Example.bf")
                             {
                                 CodeOutput.Document.Blocks.Clear();
                                 CodeOutput.Document.Blocks.Add(new Paragraph(new Run("EXE 출력중...->" + System.IO.Path.GetFileNameWithoutExtension(filePath) + ".exe")));
                                 string Erroutput = "";
                                 SaveFile();
                                 TextRange text = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
                                 번역.IsEnabled = false;
                                 Stopwatch sw = new Stopwatch();
                                 sw.Reset();
                                 sw.Start();
                                 await Task.Run(() =>
                                 {
                                     BrainFuck = new CsParser(text.Text, memsize);
                                     BrainFuck.RunCode();
                                     try
                                     {
                                         outputExE(BrainFuck.output, System.IO.Path.GetFileNameWithoutExtension(filePath), ref Erroutput);
                                         ButtonEnable();
                                     }
                                     catch (Exception E)
                                     {
                                         this.Dispatcher.BeginInvoke(new Action(() =>
                                         {
                                             SetTextBoxText(CodeOutput, "");
                                             TextRange rangeOfText1 = new TextRange(CodeOutput.Document.ContentEnd, CodeOutput.Document.ContentEnd);
                                             rangeOfText1.Text = "소스파일 출력에 실패했습니다.\n\n" + E.Message.ToString();
                                           rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.LightSkyBlue);
                                         }));
                                         ButtonEnable();
                                         return;
                                     }
                                 });
                                 sw.Stop();
                                 if (runningTime.IsChecked == true)
                                 {
                                     TextRange rangeOfText1 = new TextRange(CodeOutput.Document.ContentEnd, CodeOutput.Document.ContentEnd);
                                     rangeOfText1.Text = "\nRun time:" + sw.ElapsedMilliseconds.ToString() + "ms";
                                    rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.LightSkyBlue);
                                 }
                                 GC.Collect();
                             }
                             else
                             {
                                 SetTextBoxText(CodeOutput, "");
                                 TextRange rangeOfText1 = new TextRange(CodeOutput.Document.ContentEnd, CodeOutput.Document.ContentEnd);
                                 rangeOfText1.Text = "소스파일 출력에 실패했습니다.\n\n" + "소스파일을 먼저 저장하세요.";
                                rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.LightSkyBlue);
                             }
                         }
                         else
                         {
                             BrainFuck = new HumanParser(textRange.Text, InterPreter.CellSize.bit8, false, memsize, false, true);
                             await Task.Run(() =>
                             {
                                BrainFuck.RunCode();
                             });
                             if (BrainFuck.error == true)
                             {
                                SetTextBoxText(CodeOutput, BrainFuck.output);
                              return;
                             }
                             if (comboBox.Text == "Ook!")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new OokParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
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
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "C#")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new CsParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "C++")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new CppParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Java")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new JavaParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Swift")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new SwiftParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "As3.0")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new AsParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Python")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new PyParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "VB.NET")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new VBParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                         ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Go")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new GoParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "F#")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new FsParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Lua")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new LuaParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Rust")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new RustParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Scheme")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new SchemeParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Ruby")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new RubyParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Perl")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new PerlParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "php")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new phpParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Pascal")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new PascalParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "Object-c")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new ObjCParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                               SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                               highlightEvent(this.CodeOutput);
                             }
                             else if (comboBox.Text == "awk")
                             {
                                 번역.IsEnabled = false;
                                 BrainFuck = new awkParser(textRange.Text, memsize);
                                 await Task.Run(() =>
                                 {
                                     BrainFuck.RunCode();
                                     ButtonEnable();
                                 });
                                 SetTextBoxText(CodeOutput, BrainFuck.output);
                                 GC.Collect();
                                 highlightEvent(this.CodeOutput);

                             }
                         }
                     }
     
                await Task.Run(()=>
                {
                    Format();
                });
    
                mediaElement1.Position = TimeSpan.Zero;
                     mediaElement1.Play();
                 }
             }
   
        private async void 번역_Click(object sender, RoutedEventArgs e)
        {
            await 번역하기();
        }

  
        private void ButtonEnable()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Invoker(ButtonEnable));
                return;
            }
            번역.IsEnabled = true;
            중단.IsEnabled = false;
        }
        private void 번역_Copy_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(CodeOutput.Document.ContentStart, CodeOutput.Document.ContentEnd);
            Clipboard.SetText(textRange.Text);
        }


        private void RichTextBox_ScrollC(object sender, ScrollChangedEventArgs e)
        {
            var textToSync = (sender == CodeInput) ? LineNumLabel : CodeInput;
            textToSync.ScrollToVerticalOffset(e.VerticalOffset);
            textToSync.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void Load(object sender, RoutedEventArgs e)//파일불러오기
        {
            TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
            SaveCheck(false);
            CodeInput.Focus();
            file.LoadFile();
            if (!file.loaderr)
            {
                filePath = file.GetfilePath;
                SetTextBoxText(CodeInput, file.Getreading);
                lastFileText = textRange.Text;
            }
            else
            {
                return;
            }
        }
        private void OtherNameSave(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
            file.SaveFile(textRange.Text, true);
            filePath = file.GetfilePath;
        }

        private void SaveFile()
        {
            TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
            // MessageBox.Show(textRange.Text);
            if (filePath == "Example.bf")
            {
                file.SaveFile(textRange.Text, true);
                filePath = file.GetfilePath;
                lastFileText = textRange.Text;
            }
            else
            {
                file.SaveFile(textRange.Text, false);
                lastFileText = textRange.Text;
            }
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void PasteEvent(object sender, RoutedEventArgs e)
        {
            CodeInput.Paste();
        }

        private void recentFile_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(CodeInput.Document.ContentStart, CodeInput.Document.ContentEnd);
            MenuItem m = (MenuItem)e.OriginalSource;
            if (m.Header.ToString() == "최근사용한파일")
            {
                return;
            }
            file.LoadFileWithoutDialog(m.Header.ToString());
            CodeInput.Focus();
            filePath = file.GetfilePath;
            SetTextBoxText(CodeInput, file.Getreading);
            lastFileText = textRange.Text;
        }

        private void 번역_Copy1_Click(object sender, RoutedEventArgs e)
        {
            BrainFuck.pause = true;
            중단.IsEnabled = false;
        }

        private void 번역_Copy1_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void 설정_Click(object sender, RoutedEventArgs e)
        {
            if (optionshow == false)
            {
                runningTime.Visibility = Visibility.Visible;
                Opback.Visibility = Visibility.Visible;
                labels.Visibility = Visibility.Visible;
                bit16.Visibility = Visibility.Visible;
                bit8.Visibility = Visibility.Visible;
                bit32.Visibility = Visibility.Visible;
                lab.Visibility = Visibility.Visible;
                textBox.Visibility = Visibility.Visible;
                labelt1.Visibility = Visibility.Visible;
                labelt2.Visibility = Visibility.Visible;
                label2.Visibility = Visibility.Visible;
                dynamicMem.Visibility = Visibility.Visible;
                selep.Visibility = Visibility = Visibility.Visible;
                DynamicList.Visibility = Visibility.Visible;
                CoutingPointer.Visibility = Visibility.Visible;
                memoryView.Visibility = Visibility.Visible;
                Memorylabel.Visibility = Visibility.Visible;
            } else if (optionshow == true)
            {
                runningTime.Visibility = Visibility.Hidden;
                Opback.Visibility = Visibility.Hidden;
                labels.Visibility = Visibility.Hidden;
                bit16.Visibility = Visibility.Hidden;
                bit8.Visibility = Visibility.Hidden;
                bit32.Visibility = Visibility.Hidden;
                lab.Visibility = Visibility.Hidden;
                textBox.Visibility = Visibility.Hidden;
                labelt1.Visibility = Visibility.Hidden;
                labelt2.Visibility = Visibility.Hidden;
                label2.Visibility = Visibility.Hidden;
                dynamicMem.Visibility = Visibility.Hidden;
                selep.Visibility = Visibility.Hidden;
                DynamicList.Visibility = Visibility.Hidden;
                CoutingPointer.Visibility = Visibility.Hidden;
                memoryView.Visibility = Visibility.Hidden;
                Memorylabel.Visibility = Visibility.Hidden;
            }
            optionshow = !optionshow;
        }

        private void 도움말열기(object sender, RoutedEventArgs e)
        {
            Helper a = new Helper();
            a.Show();
        }

        private void Label_MouseDoubleClick(object sender, MouseEventArgs e)
        {
 
            SetTextBoxText(CodeInput, ">,[>,]<[.<]\n#입력받은 문자열을 뒤집어출력합니다");
        }

        private void Label_MouseDown(object sender, MouseEventArgs e)
        {
            SetTextBoxText(CodeInput, "+++++[>+++++++++<-],[[>--.++>+<<-]>+.->[<.>-]<<,]\n#입력받은 문자열을 출력하는 브레인퍽코드를 출력합니다.");
        }

        private void Label_MouseDown_1(object sender, MouseEventArgs e)
        {
            SetTextBoxText(CodeInput, @"[*로 삼각형을 만들어 출력합니다]
                                >
                               + +
                              + +
                             [ < + +
                            + +
                           + + + +
                          > -   ] >
                         + + + + + + + +
                        [               >
                       + + + +
                      < -           ] >
                     > + + >         > > + >
                    >       > +       <
                   < <     < <     < <     < <
                  <   [-   [-   > +   <
                 ] > [- < + > > > . < < ] > > >
                [                               [
               - > + +
              + + + +
             + + [ > + + + +
            < -                       ] >
           . <     < [- > + <
          ] +   >   [-   > + +
         + + + + + + + +                 < < + > ] > . [
        -               ] >               ]
       ] +             < <             < [- [
      -   > +   <           ] +           >   [
     - < + >         > > - [- > + <         ] + + >
    [-       < -       >       ] <       <
   < ] < <     < <     ] + + + + + + + + +
  +   .   + + +   .   [-   ] <   ]   + + + + +
***************************************************************");
        }

        private void Label_MouseDown_2(object sender, MouseEventArgs e)
        {
            SetTextBoxText(CodeInput, @"#구구단 2단을 출력합니다
++++++++++
[
    > +++++
    > ++++
    > +++++ +
    > ++++++++++
    <<<< -
]
>>>> +++ ++++ + +++++++++---- - ---------++++ + +++++++++---- - -------------- -++++ + ++++++++
<<<<< ++++++++++.
>
> .
> ++.
< .
>> +.
<< ++.
< ++++++++++.
> --.
> .
< +.
>> .
<< +++.
< .
> ----.
> .
< ++.
>> .
<< ++++.
< .
> ----- -.
> .
< +++.
>> .
<< ----.-.
< .
> ++.
> .
< ++++.
>> .
<< -----.+.
< .
> .
> .
< +++++.
>> .
<< ----- -. ++ +.
< .
> --.
> .
< +++++ +.
>> .
<< -------. ++++ +.
< .
> ----.
> .
< +++++++.
>> .
<< --------. ++++ + ++.");
        }

        private void Label_MouseDown_3(object sender, MouseEventArgs e)
        {
            SetTextBoxText(CodeInput, "#Hello World!를 출력시킵니다\n++++++++++[> +++++++> ++++++++++> +++> +<<<< -] > ++.> +.++++++ +..++ +.> ++.<< +++++\n++++++++++.>.++ +.------.--------.> +.>.");
        }

        private void Label_MouseDown_4(object sender, MouseEventArgs e)
        {
            SetTextBoxText(CodeInput, "#바이트값을 랜덤으로 발생시킵니다(exe모드로 실행해주세요)\n+[>.+<]");
        }

        private void Label_MouseDown_5(object sender, MouseEventArgs e)
        {
            SetTextBoxText(CodeInput, @"#피보나치 수열을 81까지 출력합니다
+++++++++++
>+>>>>++++++++++++++++++++++++++++++++++++++++++++
>++++++++++++++++++++++++++++++++<<<<<<[>[>>>>>>+>
+<<<<<<<-]>>>>>>>[<<<<<<<+>>>>>>>-]<[>++++++++++[-
<-[>>+>+<<<-]>>>[<<<+>>>-]+<[>[-]<[-]]>[<<[>>>+<<<
-]>>[-]]<<]>>>[>>+>+<<<-]>>>[<<<+>>>-]+<[>[-]<[-]]
>[<<+>>[-]]<<<<<<<]>>>>>[+++++++++++++++++++++++++
+++++++++++++++++++++++.[-]]++++++++++<[->-<]>++++
++++++++++++++++++++++++++++++++++++++++++++.[-]<<
<<<<<<<<<<[>>>+>+<<<<-]>>>>[<<<<+>>>>-]<-[>>.>.<<<
[-]]<<[>>+>+<<<-]>>>[<<<+>>>-]<<[<+>-]>[<+>-]<<<-]");
        }

        private void Label_MouseDown_6(object sender, MouseEventArgs e)
        {
            SetTextBoxText(CodeInput, @"#입력받은 브레인퍽코드를 c언어로 출력합니다
+++[>+++++<-]>>+<[>>++++>++>+++++>+++++>+>>+<++[++<]>---]

>++++.>>>.+++++.>------.<--.+++++++++.>+.+.<<<<---.[>]<<.<<<.-------.>++++.
<+++++.+.>-----.>+.<++++.>>++.>-----.

<<<-----.+++++.-------.<--.<<<.>>>.<<+.>------.-..--.+++.-----<++.<--[>+<-]
>>>>>--.--.<++++.>>-.<<<.>>>--.>.

<<<<-----.>----.++++++++.----<+.+++++++++>>--.+.++<<<<.[>]<.>>

,[>>+++[<+++++++>-]<[<[-[-<]]>>[>]<-]<[<+++++>-[<+++>-[<-->-[<+++>-
[<++++[>[->>]<[>>]<<-]>[<+++>-[<--->-[<++++>-[<+++[>[-[-[-[->>]]]]<[>>]<<-]
>[<+>-[<->-[<++>-[<[-]>-]]]]]]]]]]]]]

<[
    -[-[>+<-]>]
    <[<<<<.>+++.+.+++.-------.>---.++.<.>-.++<<<<.[>]>>>>>>>>>]
    <[[<]>++.--[>]>>>>>>>>]
    <[<<++..-->>>>>>]
    <[<<..>>>>>]
    <[<<..-.+>>>>]
    <[<<++..---.+>>>]
    <[<<<.>>.>>>>>]
    <[<<<<-----.+++++>.----.+++.+>---.<<<-.[>]>]
    <[<<<<.-----.>++++.<++.+++>----.>---.<<<.-[>]]
    <[<<<<<----.>>.<<.+++++.>>>+.++>.>>]
    <.>
]>
,]

<<<<<.<+.>++++.<----.>>---.<<<-.>>>+.>.>.[<]>++.[>]<.");
        }
        private void Label_MouseDown_7(object sender, MouseEventArgs e)
        {
            SetTextBoxText(CodeInput, @"#99병의 맥주의 가사를 출력합니다
+++++++++>+++++++++>>>++++++[<+++>-]+++++++++>+++++++++>>>>>>+++++++++++++[<+++
+++>-]>+++++++++++[<++++++++++>-]<+>>++++++++[<++++>-]>+++++++++++[<++++++++++>
-]<->>+++++++++++[<++++++++++>-]<+>>>+++++[<++++>-]<-[<++++++>-]>++++++++++[<++
++++++++>-]<+>>>+++++++[<+++++++>-]>>>++++++++[<++++>-]>++++++++[<++++>-]>+++++
++++++[<+++++++++>-]<->>++++++++[<++++>-]>+++++++++++[<++++++++++>-]<+>>+++++++
+[<++++>-]>>+++++++[<++++>-]<+[<++++>-]>++++++++[<++++>-]>>+++++++[<++++>-]<+[<
++++>-]>>++++++++++++[<+++++++++>-]++++++++++>>++++++++++[<++++++++++>-]<+>>+++
+++++++++[<+++++++++>-]>>++++++++++++[<+++++++++>-]>>++++++[<++++>-]<-[<+++++>-
]>++++++++++++[<++++++++>-]<+>>>>++++[<++++>-]<+[<+++++++>-]>++++++++[<++++>-]>
++++++++[<++++>-]>+++++++++++[<++++++++++>-]<+>>++++++++++[<++++++++++>-]<+>>>+
+++[<++++>-]<+[<++++++>-]>+++++++++++++[<++++++++>-]>++++++++[<++++>-]>>+++++++
[<++++>-]<+[<++++>-]>+++++++++++[<+++++++++>-]<->>++++++++[<++++>-]>++++++++++[
<++++++++++>-]<+>>+++++++++++[<++++++++++>-]>++++++++++[<++++++++++>-]<+>>+++++
++++++[<++++++++++>-]<+>>>+++++[<++++>-]<-[<++++++>-]>++++++++[<++++>-]>+++++++
+++>>>>++++++++++++[<+++++++>-]++++++++++>>++++++++++++[<++++++++>-]<+>>+++++++
+++[<++++++++++>-]>++++++++++++[<+++++++++>-]<->>+++++++++++[<++++++++++>-]>+++
+++++++[<++++++++++>-]<+>>+++++++++++++[<+++++++++>-]>++++++++[<++++>-]>+++++++
++++[<++++++++++>-]<+>>>>+++++[<++++>-]<-[<++++++>-]>+++++++++++[<++++++++++>-]
<+>>++++++++++++[<++++++++>-]<+>>+++++++++++[<++++++++++>-]>++++++++[<++++>-]>+
+++++++++[<++++++++++>-]<+>>>+++++++[<++++>-]<+[<++++>-]>>>+++++[<+++>-]<[<++++
+++>-]>>+++++[<+++>-]<[<+++++++>-]>++++++++[<++++>-]>>+++++++[<++++>-]<+[<++++>
-]>>++++++[<++++>-]<-[<+++++>-]>>>++++++[<++++>-]<-[<+++++>-]>++++++++[<++++>-]
>++++++++++++[<++++++++>-]<+>>++++++++++[<++++++++++>-]>>++++[<++++>-]<[<++++++
+>-]>+++++++++++[<++++++++++>-]<+>>++++++++[<++++>-]>>++++[<++++>-]<+[<+++++++>
-]>++++++++++[<++++++++++>-]>+++++++++++[<++++++++++>-]>+++++++++++[<++++++++++
>-]>++++++++[<++++>-]>++++++++++++[<++++++++>-]<+[<]<[<]<[<]<[<]<<<<[<]<[<]<[<]
<[<]<<<<[<]<<<<<<[>>[<<[>>>+>+<<<<-]>>>[<<<+>>>-]<[>+<-]>>-[>-<[-]]>+[<+>-]<<<]
>>[<<<<[-]>>>>>>>[>]>.>>>[.>>]>[>]>>[.>>]<[.<<]<[<]<<.>>>[.>>]>[>]>>[.>>]>.>>>[
.>>]>[>]>>[.>>]>>[.>>]<[.<<]<<<<[<]<[<]<[<]<[<]<<<<[<]>[.>]>>>>[.>>]>>[.>>]>>[.
>>]<[.<<]<[<]<<<<[<]<<-]<<<<[[-]>>>[<+>-]<<[>>+>+<<<-]>>>[<<<+>>>-]<[<<<+++++++
+[>++++++<-]>>>[-]]++++++++[<++++++>-]<<[.>.>]>[.>>]>>>[>]>>>>[.>>]>>[.>>]>>[.>
>]<[.<<]<[<]<<<<[<]<<<<<[.>.>]>[.>>]<<<[-]>[-]>>>>>[>]>>>>[.>>]>>[.>>]>>[.>>]>.
>>>[.>>]>>[.>>]>[>]>>[.>>]<[.<<]<<<<[<]<[<]<[<]<[<]<<<<[<]<<<<<<]<<[>+>+<<-]>>[
<<+>>-]<[>-<[-]]>+[<+>-]<[<++++++++++<->>-]<<[>>+>+>+<<<<-]>>[<<+>>-]<-[>+>+>>+
<<<<-]>[<+>-]>>>[<<[>>>+>+<<<<-]>>>[<<<+>>>-]<[>+<-]>>-[>-<[-]]>+[<+>-]<<<]>>[<
<<<[-]>>>>>>>[>]>.>>>[.>>]>[>]>>[.>>]<[.<<]<[<]<<<<[<]<<-]<<<<[[-]>>>[<+>-]<<[>
>+>+<<<-]>>>[<<<+>>>-]<[<<<++++++++[>++++++<-]>>>[-]]++++++++[<++++++>-]<<[.>.>
]>[.>>]<<<[-]>[-]>>>>>[>]>>>>[.>>]>>[.>>]>>[.>>]<[.<<]<[<]<<<<[<]<<<<<<]+++++++
+++.[-]<<<[>>+>+>+<<<<-]>>[<<+>>-]<[>+>+>>+<<<<-]>[<+>-]>]
");
        }

        private void Label_MouseDown_8(object sender, MouseEventArgs e)
        {
            SetTextBoxText(CodeInput, @"#입력받은 숫자에따라 일정하게 선을 그립니다
>>>>+>+++>+++>>>>>+++>>+[
    -,[----------[---[+<++++[>-----<-]+>[<+>--------[<+>-
    [--->>+++++++++++++[<<[-<+>>]>[<]>-]<<
    [+>+++++[<-------->-]<[<+>-]]]]]]]]
    <
    [<<++[>>>>>>>>>>>+<<<<<<<<<<<-]<<+>+>+>>>+>+>>+>+<<<<<-
    [<<+>>>+>+>>>+<<<<<-
    [<<<<+>>->>>>->>+<<<<-
    [<<<<->+>>>>->>>->-<<<<<-
    [<<<->>>>+<-
    [<<<+>>>>->+>>+<<<<-
    [<<<<+>->+>>>+>>>>+<<<<<-
    [<<->>>->->>>-<<<<<-
    [<<<<->+>>>>+>+>>>+<<<<<-
    [<<<<+>>>>>>-<<-
    [<<+>>>->>>>-<<<<<-
    [>+>>>->+<<<<<-
    [>>+<<-
    [<<<->->>>->->>+<<<<-
    [<<<+>+>>>+>+<<-
    [>->-<<-
    [<<->>>+>+<<-
    [<<+>>>>>>->-<<<<<-
    [<<<<->>->>-
    [<<<<+>>>>>>>>+<<<<-
    [<<<<->>+>>>>>>>+<<<<<-
    [>->>>-<<<<-]]]]]]]]]]]]]]]]]]]]]
    >[[<<<<<<<<<<<+>>>>>>>>>>>-]>]+>>>>>>>+>>+<]>
]<<[-]<[-[>>>>+<<<<-]]<<<<<<++<+++<+++[>]<[
    >>>>>++++++++[<++++++<++++>>-]>>>[-[<+<<<<.>>>>>-]]<<+<<-<<<<[
        -[-[>+<-]>]>>>[.[>]]<<[<+>-]>>>[<<-[<++>-]>>-]
        <<[++[<+>--]>+<]>>>[<+>-]<<<<<<<<
    ]>>>>>++++++++++.>+[[-]<]<<<
]");
        }
    }
    }
