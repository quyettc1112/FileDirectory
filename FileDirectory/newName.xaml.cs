
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
            if (tb_newpath.Text != null) {
                var resutl = MessageBox.Show($"Save {tb_newpath}", "Info", MessageBoxButton.YesNo);
                if (resutl == MessageBoxResult.Yes) { 
                    
                }
            }
            else
            {
                MessageBox.Show("Empty Folder Name", "Error");
            }


        }
    }
}
