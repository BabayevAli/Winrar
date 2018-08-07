using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3;

namespace WpfApp3
{
    public class Compress
    {
        string pathIn;
        string filename;
        CompressFormat format;
        int countThreads;
        public Compress(string pathIn, string newFileName = "NewFile", CompressFormat compressFormat = CompressFormat.gz, int threads = 4)
        {
            this.pathIn = pathIn;
            countThreads = threads;
            if (newFileName == "NewFile")
                newFileName += DateTime.Now.ToString();
            filename = newFileName;
            format = compressFormat;
        }
        
        public void setWindow(MainWindow main)
        {
            window = main;
        }

        MainWindow window;



        byte[] data;
        List<Task> taskList = new List<Task>();
        List<byte[]> dataSplited = new List<byte[]>();
        Dictionary<int, byte[]> listZipBytes = new Dictionary<int, byte[]>();
        List<Task> listTask;
        public string Start()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (FileStream inFile = new FileStream(pathIn, FileMode.Open))
            {
                data = new byte[inFile.Length];
                inFile.Read(data, 0, data.Length);
                dataSplited = splitBytes(inFile.Length);

                listTask = new List<Task>();

                for (int i = 0; i < dataSplited.Count; i++)
                {
                    listTask.Add(Task.Factory.StartNew(() => { compevent(); }));
                }
                Task.WaitAll(listTask.ToArray());
                string filePath = pathIn.Substring(0, pathIn.LastIndexOf('\\') + 1) + filename + '.' + format.ToString();
                writeToFile(filePath);
            }
            stopwatch.Stop();
            Clear();
            return stopwatch.ElapsedMilliseconds.ToString() + "ms";
        }

      
        public int getTaskID(int id)
        {
            for (int i = 0; i < listTask.Count; i++)
            {
                if (listTask[i].Id == id)
                {
                    return i;
                }
            }
            return 0;
        }

        void compevent()
        {
            int position = getTaskID(Task.CurrentId.Value);
            using (MemoryStream comp = new MemoryStream())
            {
                using (GZipStream inStream = new GZipStream(comp, CompressionMode.Compress))
                {
                    window.ChangeMaxValue(position, dataSplited[position].Length);
                    for (int i = 0; i < dataSplited[position].Length; i++)
                    {
                        window.AddValue(position, i+1);
                        inStream.WriteByte(dataSplited[position][i]);
                    }
                }
                listZipBytes.Add(position, comp.ToArray());
            }
        }

        public void SetValueToString(int position,int value)
        {
            switch (position)
            {
                case 0: window.Thread1.Text = "Thread1 :" + value.ToString(); break;
                case 1: window.Thread2.Text = "Thread2 :" + value.ToString(); break;
                case 2: window.Thread3.Text = "Thread3 :" + value.ToString(); break;
                case 3: window.Thread4.Text = "Thread4 :" + value.ToString(); break;
                case 4: window.Thread5.Text = "Thread5 :" + value.ToString(); break;
            }
        }

        public List<byte[]> splitBytes(long length)
        {
            int part = (int)(length / countThreads);
            List<byte[]> listBytes = new List<byte[]>();
            window.CountsAll.Text = "Infile Bytes count:" + length.ToString();
            int b = 0;
            for (int i = 0; i < countThreads; i++)
            {
                listBytes.Add(data.Skip(part * i).Take(part).ToArray());
                SetValueToString(b, part);
                b ++;
            }
            byte[] ada = data.Skip(part * countThreads).Take(data.Length - part).ToArray();
            if (ada.Length != 0)
            {
                SetValueToString(b, ada.Length);
                listBytes.Add(ada);
            }
            return listBytes;
        }

        public void writeToFile(string pth)
        {
            using (FileStream fileStreamResult = new FileStream(pth, FileMode.Create))
            {
                for (int i = 0; i < dataSplited.Count; i++)
                {
                    fileStreamResult.Write(listZipBytes[i], 0, listZipBytes[i].Length);
                }
            }
        }

        public void Clear()
        {
            taskList.Clear();
            dataSplited.Clear();
            listZipBytes.Clear();
            pathIn = "";
            filename = "";
            format = CompressFormat.gz;
            countThreads = 1;
        }
    }
}
