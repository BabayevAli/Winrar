using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    class Decompress
    {
        string pathIn;
        int countThreads;
        public Decompress(string pathIn, int threads = 1)
        {
            this.pathIn = pathIn;
            countThreads = threads;
        }

        MainWindow window;
       
        public void setWindow(MainWindow main)
        {
            window = main;
        }

        byte[] data;
        List<byte[]> dataSplited = new List<byte[]>();
        Dictionary<int, List<byte[]>> decompresslistbytes;
        List<Task> listTask;
        public string Start()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (FileStream inFile = new FileStream(pathIn, FileMode.Open))
            {
                decompresslistbytes = new Dictionary<int, List<byte[]>>();
                data = new byte[inFile.Length];
                inFile.Read(data, 0, data.Length);
                window.CountsAll.Text = "Infile Bytes count:" + data.Length.ToString();

                dataSplited = SplitByte();

                listTask = new List<Task>();

                for (int i = 0; i < dataSplited.Count; i++)
                {
                    listTask.Add(Task.Factory.StartNew(() => { deCompEvent(); }));
                }
                Task.WaitAll(listTask.ToArray());
                writeToFile();
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString() + "ms";
        }

        public void Clear()
        {
            dataSplited.Clear();
            decompresslistbytes.Clear();
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
        void deCompEvent()
        {
            int position = getTaskID(Task.CurrentId.Value);
            List<byte[]> listGrb = new List<byte[]>();
            using (MemoryStream fileStream = new MemoryStream(dataSplited[position]))
            {
                using (GZipStream gZipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                {
                    byte[] bytes = new byte[10];
                    var bytesCount = 0;
                    while ((bytesCount = gZipStream.Read(bytes, 0, 10)) != 0)
                    {
                        listGrb.Add(bytes.Take(bytesCount).ToArray());
                    }
                    window.AddValue(position, 100);
                    decompresslistbytes.Add(position, listGrb);
                }
            }
        }

        public void writeToFile()
        {
            using (FileStream fileStreamResult = new FileStream(pathIn.Substring(0, pathIn.LastIndexOf('.')), FileMode.Create))
            {
                for (int i = 0; i < decompresslistbytes.Count; i++)
                {
                    List<byte[]> bytes = decompresslistbytes[i];
                    foreach (var byteinone in bytes)
                    {
                        fileStreamResult.Write(byteinone, 0, byteinone.Length);
                    }
                }
            }
        }

        public byte[] StringToByte(string str)
        {
            var list = str.Split(' ').ToList();
            list.RemoveAt(list.Count - 1);
            var byts = new byte[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                byts[i] = byte.Parse(list[i]);
            }
            return byts;
        }
        public void SetValueToString(int position, int value)
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
        public List<byte[]> SplitByte()
        {
            string datas = "";
            string header = "";
            for (int i = 0; i < data.Length; i++)
            {
                if (i < 10)
                    header += data[i] + " ";
                datas += data[i];
                if (i + 1 != data.Length)
                    datas += " ";
            }
            var lists = datas.Split(new string[] { header }, StringSplitOptions.None).ToList();
            List<byte[]> bytes = new List<byte[]>();
            lists.RemoveAt(0);
            for (int i = 0; i < lists.Count; i++)
            {
                lists[i] = header + lists[i];
                SetValueToString(i, lists[i].Length);
                bytes.Add(StringToByte(lists[i]));
            }
            
            return bytes;
        }


    }
}
