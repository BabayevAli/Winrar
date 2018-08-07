using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApp3
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region view    
        private long progress1;

        public long Progress1
        {
            get { return progress1; }
            set
            {
                progress1 = value;
                NotifyPropertyChanged("Progress1");
            }
        }

        private long progress2;

        public long Progress2
        {
            get { return progress2; }
            set
            {
                progress2 = value;
                NotifyPropertyChanged("Progress2");
            }
        }

        private long progress3;

        public long Progress3
        {
            get { return progress3; }
            set
            {
                progress3 = value;
                NotifyPropertyChanged("Progress3");
            }
        }

        private long progress4;

        public long Progress4
        {
            get { return progress4; }
            set
            {
                progress4 = value;
                NotifyPropertyChanged("Progress4");
            }
        }

        private long progress5;

        public long Progress5
        {
            get { return progress5; }
            set
            {
                progress5 = value;
                NotifyPropertyChanged("Progress5");
            }
        }

        private long maxValue1 = 100;

        public long MaxValue1
        {
            get { return maxValue1; }
            set { maxValue1 = value; NotifyPropertyChanged("MaxValue1"); }
        }

        private long maxValue2 = 100;

        public long MaxValue2
        {
            get { return maxValue2; }
            set { maxValue2 = value; NotifyPropertyChanged("MaxValue2"); }
        }
        private long maxValue3 = 100;

        public long MaxValue3
        {
            get { return maxValue3; }
            set { maxValue3 = value; NotifyPropertyChanged("MaxValue3"); }
        }

        private long maxValue5 = 100;

        public long MaxValue5
        {
            get { return maxValue5; }
            set { maxValue5 = value; NotifyPropertyChanged("MaxValue5"); }
        }

        private long maxValue4 = 100;

        public long MaxValue4
        {
            get { return maxValue4; }
            set { maxValue4 = value; NotifyPropertyChanged("MaxValue4"); }
        }


        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                Path.Text = filename;
            }
        }

        public void AddValue(int position,int value)
        {
            switch (position)
            {
                case 0: Progress1 = value; break;
                case 1: Progress2 = value; break;
                case 2: Progress3 = value; break;
                case 3: Progress4 = value; break;
                case 4: Progress5 = value; break;
            }
        }

        public void ChangeMaxValue(int position, int value)
        {
            switch (position)
            {
                case 0: MaxValue1 = value; break;
                case 1: MaxValue2 = value; break;
                case 2: MaxValue3 = value; break;
                case 3: MaxValue4 = value; break;
                case 4: MaxValue5 = value; break;
            }
        }

        private void compress(object sender, RoutedEventArgs e)
        {
            ResetAll();
            status.Text = "Working...";
            try
            {
                Window1 window1 = new Window1();
                window1.ShowDialog();
                Compress compress = new Compress(Path.Text,window1.Filename,window1.Format,int.Parse(CountsThreads.Text));
                compress.setWindow(this);
                times.Text = compress.Start();
                status.Text = "Completed!!";
            }
            catch (Exception error)
            {
                status.Text = error.Message;
            }
        }

        public void ResetAll()
        {
            Progress1 = 0;
            Progress2 = 0;
            Progress3 = 0;
            Progress4 = 0;
            Progress5 = 0;
            MaxValue1 = 100;
            MaxValue2 = 100;
            MaxValue3 = 100;
            MaxValue4 = 100;
            MaxValue5 = 100;
            CountsAll.Text = "Infile Bytes count:";
            Thread1.Text = "Thread1 : ";
            Thread2.Text = "Thread2 : ";
            Thread3.Text = "Thread3 : ";
            Thread4.Text = "Thread4 : ";
            Thread5.Text = "Thread5 : ";
        }

        private void deCompress(object sender, RoutedEventArgs e)
        {
            ResetAll();
            status.Text = "Working...";
            try
            {
                Decompress decompress = new Decompress(Path.Text, int.Parse(CountsThreads.Text));
                decompress.setWindow(this);
                decompress.Start();
                status.Text = "Completed!!";
            }
            catch (Exception error)
            {
                status.Text = error.Message;
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                Path.Text = files[0];
            }
        }
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
