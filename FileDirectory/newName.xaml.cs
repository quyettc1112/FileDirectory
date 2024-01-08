
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

 

        public string Path { get; set; }
        public string Name { get; set; }

        public Window1(string Path, string Name )
        {
            InitializeComponent();
            this.Path = Path;
            this.Name = Name;
            tb_newpath.Text = this.Name;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] pathLine = this.Path.Split('\\');

            if (tb_newpath.Text != null) {
                var resutl = MessageBox.Show($"Save {tb_newpath.Text}", "Info", MessageBoxButton.YesNo);
                if (resutl == MessageBoxResult.Yes) {
                    try {
                        RenameDirectory(pathLine);
                        RenameFile(pathLine);   

                    } catch(Exception ex) {
                            MessageBox.Show($"Error: {ex.Message}", "Error");                     
                    }

                }
            }
            else
            {
                MessageBox.Show("Empty Folder Name", "Error");
            }
        }

        public void RenameDirectory(string[] pathLine) {
            if (Directory.Exists(this.Path) && IsDirectory(this.Path))
            {
                pathLine[pathLine.Length - 1] = tb_newpath.Text;

                StringBuilder concatenatedPath = new StringBuilder();

                foreach (string pathSegment in pathLine)
                {
                    // Nếu chuỗi đã có giá trị, thêm dấu phân cách (ví dụ: dấu gạch chéo) trước khi thêm phần tử mới.
                    if (concatenatedPath.Length > 0)
                    {
                        concatenatedPath.Append("\\");
                    }
                    concatenatedPath.Append(pathSegment);
                }

                string finalPath = concatenatedPath.ToString();

                // Đổi tên thư mục hoặc tập tin
                Directory.Move(this.Path, finalPath);

                var resutl = MessageBox.Show(finalPath.ToString(), "Info", MessageBoxButton.OK);
                if (resutl == MessageBoxResult.OK) { 
                    this.Close();
                }
                
            }
        
        }
        public void RenameFile(string[] pathLine) {
            if (File.Exists(this.Path) && IsDirectory(this.Path) == false)
            {
                pathLine[pathLine.Length - 1] = tb_newpath.Text;

                StringBuilder concatenatedPath = new StringBuilder();

                foreach (string pathSegment in pathLine)
                {
                    // Nếu chuỗi đã có giá trị, thêm dấu phân cách (ví dụ: dấu gạch chéo) trước khi thêm phần tử mới.
                    if (concatenatedPath.Length > 0)
                    {
                        concatenatedPath.Append("\\");
                    }
                    concatenatedPath.Append(pathSegment);
                }

                string finalPath = concatenatedPath.ToString();

                // Đổi tên thư mục hoặc tập tin
                File.Move(this.Path, finalPath);
                var resutl = MessageBox.Show(finalPath.ToString(), "Info", MessageBoxButton.OK);
                if (resutl == MessageBoxResult.OK)
                {
                  
                    this.Close();
                    

                }
            }
        
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
