using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using Microsoft.Win32;

namespace BTF
{
    public class FileSystem
    {
        private string filePath;
        private Queue<string> recentFilepath=new Queue<string>();
        private const int Qlimit=10;
      
        private string reading;
        public string Getreading { get { return this.reading; } set { value = this.reading; } }
        public string GetfilePath { get { return this.filePath; } set { value = this.filePath; } }
        public FileSystem(ref Queue<string> fileQue)
        {
            fileQue=recentFilepath;
        }
        public async void LoadFile(string filter = "BTF Files (*.btf)|*.btf")//파일불러오기
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter =filter;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
              
                using (System.IO.StreamReader sr = new System.IO.StreamReader(dlg.FileName))
                {
                    await Task.Run(() =>
                    {
                        this.reading = sr.ReadToEnd();
                        this.filePath = dlg.FileName;
                        if (recentFilepath.Count == Qlimit)
                        {
                            recentFilepath.Dequeue();
                            recentFilepath.Enqueue(filePath);
                        }
                        else if (recentFilepath.Count < Qlimit)
                        {
                            recentFilepath.Enqueue(filePath);
                        }
                    });
                }
            }
        }
        public async void SaveFile(string text,bool useDialog,string fileName = "untitled", string defaultExt = ".btf", string filter = "BTF Files(*.btf)|*.btf")
        {
            if (useDialog)
            {
                SaveFileDialog Savecode = new SaveFileDialog();
                string dir = "";
                Savecode.FileName = fileName;
                Savecode.DefaultExt = defaultExt;
                Savecode.Filter = filter;
                bool? result = Savecode.ShowDialog();
                if (result == true)
                {
                    dir = Savecode.FileName;
                    FileStream fs = new FileStream(dir, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    await sw.WriteLineAsync(text); // 파일 저장
                    filePath = Savecode.FileName;
                    if (recentFilepath.Count == Qlimit)
                    {
                        recentFilepath.Dequeue();
                        recentFilepath.Enqueue(filePath);
                    }else if (recentFilepath.Count < Qlimit)
                    {
                        recentFilepath.Enqueue(filePath);
                    }
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }
            else if(!useDialog)
            {
                StreamWriter sw = new StreamWriter(filePath, false);
                await sw.WriteAsync(text); // 파일 저장
                sw.Flush();
                sw.Close();
            }
            }
        }
    }


