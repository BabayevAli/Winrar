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

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string Filename { get; set; }
        public CompressFormat Format { get; set; }
        public Window1()
        {
            InitializeComponent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Filename = FileName.Text;
            switch (Cmb.Text)
            {
                case ".gz": Format = CompressFormat.gz; break;
                case ".zip": Format = CompressFormat.zip; break;
                case ".rar": Format = CompressFormat.rar; break;
            }
            Close();
        }
    }
}
