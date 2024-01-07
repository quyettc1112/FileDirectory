using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Winforms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;

namespace FileDirectory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<MyData> DataList { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            tb_filedirectory.Text = "";
        }

        private void btn_name_Click(object sender, RoutedEventArgs e)
        {
            Winforms.FolderBrowserDialog dialog = new Winforms.FolderBrowserDialog();
            Winforms.DialogResult dialogResult = dialog.ShowDialog();
            DataList = new ObservableCollection<MyData>
            {


            };

            if (dialogResult == Winforms.DialogResult.OK)
            {
                string path = dialog.SelectedPath;
                tb_filedirectory.Text = path;
                DataList.Clear();
                try
                {
                    string[] files = Directory.GetFiles(path);
                    string[] folder = Directory.GetDirectories(path);
                    
                    lv_listfile.Items.Clear();
                    
                    foreach (string file in files)
                    {
                        string[] result = file.Split('\\');
                        DataList.Add(new MyData { FolderName = $"{result[result.Length - 2]}", FileName = $"{result[result.Length - 1]}", FolderIcon = "" });
                    };
                    lv_listfile.ItemsSource = DataList;
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
                }
            }
            else {
                MessageBox.Show("Cancel select file","Cancel");
                tb_filedirectory.Text = "";
                lv_listfile.Items.Clear();
            }
        }

        private void lv_listfile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selected = lv_listfile.SelectedItem.ToString();  
        }

        public class MyData
        {
            public string FolderName { get; set; }
            public string FileName { get; set; }
            public string FolderIcon { get; set; }
        }
    }
}
