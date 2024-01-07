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
            
            tb_filedirectory.Text = "";
           
            DataList = new ObservableCollection<MyData>
            {


            };
            DataList.Clear();
      
 

            Winforms.FolderBrowserDialog dialog = new Winforms.FolderBrowserDialog();
            Winforms.DialogResult dialogResult = dialog.ShowDialog();

            if (dialogResult == Winforms.DialogResult.OK)
            {
            
                string path = dialog.SelectedPath;
                tb_filedirectory.Text = path;
         
                try
                {
                    string[] files = Directory.GetFiles(path);
                    string[] folder = Directory.GetDirectories(path);
                    
                
                    foreach (string file in folder)
                    {
                        string[] result = file.Split('\\');
                        DataList.Add(new MyData
                        {
                            Name = $"{result[result.Length - 1]}",
                            Path = $"{file}",
                            Icon = "C:\\Users\\Admin\\Desktop\\PRN221\\Fileee\\vector-folder-icon.jpg"
                        });
                    };
                  
                    foreach (string file in files)
                    {
                        string[] result = file.Split('\\');
                        DataList.Add(new MyData {
                            Name = $"{result[result.Length - 1]}", 
                            Path = $"{file}",
                            Icon = "C:\\Users\\Admin\\Desktop\\PRN221\\Fileee\\file-icon.png"
                        });
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
               
                DataList.Clear();
                lv_listfile.ItemsSource = DataList;
                tb_filedirectory.Text = "";
            }
        }

        private void lv_listfile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selected = lv_listfile.SelectedItem.ToString();  
        }

        public class MyData
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string Icon { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = lv_listfile.SelectedIndex;

            if (lv_listfile.SelectedIndex != -1)
            {
                // Lấy dường dẫn của Item
                string path = DataList[selectedIndex].Path;
     
                // Check File có tồn tại trong ổ đĩa ko
                if (File.Exists(path))
                {
                    var result= MessageBox.Show($"{DataList[selectedIndex].Name} will remove", "Info", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes) {
                        File.Delete(path);
                        DataList.RemoveAt(selectedIndex);
                        lv_listfile.ItemsSource = null;
                        lv_listfile.ItemsSource = DataList;
                    }
                }
                else {
                    MessageBox.Show("No File Fould", "Error");
                }
            }
            else {
                MessageBox.Show("Select File To Delete", "Error");
            }
           


        }


    }
}
