using System;
using System.Collections.Generic;
using System.IO;
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

namespace FileDirectory
{
    /// <summary>
    /// Interaction logic for newFolderOrFiletxt.xaml
    /// </summary>
    public partial class newFolderOrFiletxt : Window
    {

        public string Path { get; set; }
        public Boolean fof { get; set; }
        public newFolderOrFiletxt(string path, Boolean f0f)
        {
            InitializeComponent();
            Path = path;
            this.fof = f0f; 
        }

        private void btn_savefof_Click(object sender, RoutedEventArgs e)
        {
            string nameFolder = tb_fof.Text;
            if (fof && nameFolder != string.Empty && Path != string.Empty) {
                if (!Directory.Exists(Path +$"\\{nameFolder}"))
                {
                    Directory.CreateDirectory(Path+$"\\{nameFolder}");
                    MessageBox.Show("Create Folder Success", "Info");
                    this.Close();
                }
                else {
                    MessageBox.Show("Dupplicated Folder", "Error");
                }
            }

            if (fof == false && nameFolder != string.Empty && Path != string.Empty)
            {
                if (!File.Exists(Path + $"\\{nameFolder}"))
                {
                    using (StreamWriter sw = File.CreateText(Path + $"\\{nameFolder}")) {
                        sw.WriteLine("Đây là nội dung của tệp văn bản mới.");
                    }
                    MessageBox.Show("Create Files Success", "Info");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dupplicated Folder", "Error");
                }
            }

        }

      
    }
}
