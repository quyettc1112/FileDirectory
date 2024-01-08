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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.Integration;

namespace FileDirectory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {

        Winforms.FolderBrowserDialog dialog = new Winforms.FolderBrowserDialog();

        public ObservableCollection<MyData> DataList { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            tb_filedirectory.Text = "";

        }

        string path = string.Empty;
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
                path = dialog.SelectedPath;
                tb_filedirectory.Text = path;
                try
                {
                    ReadFileorFolder(path);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
                }
            }
            else {
                MessageBox.Show("Cancel select file", "Cancel");

                DataList.Clear();
                lv_listfile.ItemsSource = DataList;
                tb_filedirectory.Text = "";
            }
        }

        private void lv_listfile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = lv_listfile.SelectedIndex;        
            string pathToOpen = DataList[selectedIndex].Path;

            
            tb_filedirectory.Text = pathToOpen;
            path = pathToOpen;
            if (IsDirectory(path) == true) {
                ReadFileorFolder(path);
            }
            
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
                // Check đường dẫn nó là file hay Derictory


                if (IsDirectory(path) && Directory.Exists(path))
                {
                    var result = MessageBox.Show($"{DataList[selectedIndex].Name} will remove", "Info", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        Directory.Delete(path);
                        DataList.RemoveAt(selectedIndex);
                        lv_listfile.ItemsSource = null;
                        lv_listfile.ItemsSource = DataList;
                    }
                }

                if (IsDirectory(path) == false && File.Exists(path))
                {
                    var result = MessageBox.Show($"{DataList[selectedIndex].Name} will remove", "Info", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        File.Delete(path);
                        DataList.RemoveAt(selectedIndex);
                        lv_listfile.ItemsSource = null;
                        lv_listfile.ItemsSource = DataList;
                    }
                }
            }
            else {
                MessageBox.Show("Select File To Delete", "Error");
            }
        }


        public void ReadFileorFolder(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] folder = Directory.GetDirectories(path);
            DataList.Clear();

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
                DataList.Add(new MyData
                {
                    Name = $"{result[result.Length - 1]}",
                    Path = $"{file}",
                    Icon = "C:\\Users\\Admin\\Desktop\\PRN221\\Fileee\\file-icon.png"
                });
            };
            lv_listfile.ItemsSource = DataList;


        }

        

        public static bool IsDirectory(string path)
        {
            try
            {
                // Kiểm tra xem path có phải là một thư mục không
                FileAttributes attr = File.GetAttributes(path);
                return (attr & FileAttributes.Directory) == FileAttributes.Directory;
            }
            catch (Exception)
            {
                // Xảy ra lỗi khi kiểm tra, giả sử không phải là thư mục
                return false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int selectedIndex = lv_listfile.SelectedIndex;
            if (selectedIndex != -1) {
                var itemRename = DataList[selectedIndex];
                Window1 newName = new Window1(itemRename.Path, itemRename.Name);
                newName.Closing += (s, ea) =>
                {
                    ReadFileorFolder(path);
                    tb_filedirectory.Text = path;
                };
                newName.Show();
            } 
            


        }

  
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {



            // C:// a/b/c
            string[] pathArray = path.Split('\\');
            string result = "";
            if (pathArray.Length > 1)
            {
                for (int i = 0; i < pathArray.Length - 1; i++)
                {
                    if (i == pathArray.Length - 2)
                    {
                        result = result + pathArray[i];
                    }
                    else
                        result = result + pathArray[i] + "\\";
                }
                path = result;
                tb_filedirectory.Text = path;
                ReadFileorFolder(path);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("Create Folder:", "Create", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (path != string.Empty)
            {
                newFolderOrFiletxt nfof = new newFolderOrFiletxt(path, false);

                nfof.Closing += (s, ea) =>
                {
                    ReadFileorFolder(path);
                    tb_filedirectory.Text = path;
                };
                nfof.ShowDialog();
            }
           
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            if (path != string.Empty) {
                newFolderOrFiletxt nfof = new newFolderOrFiletxt(path, true);
                nfof.Closing += (s, ea) =>
                {
                    ReadFileorFolder(path);
                    tb_filedirectory.Text = path;
                };
                nfof.ShowDialog();
            }
            
        }
    }
}
