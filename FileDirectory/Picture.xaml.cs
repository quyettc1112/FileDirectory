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
    /// Interaction logic for Picture.xaml
    /// </summary>
    /// 

    

    public partial class Picture : Window
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public Picture(string path, string name)
        {
            InitializeComponent();
            Path = path;
            Name = name;  
            
            tb_name.Text = name;
            SetImageSource(path);
            
        }


        private void SetImageSource(string imagePath)
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath);
                bitmapImage.EndInit();

                // Assign the BitmapImage to your Image control
                image.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                // Handle exceptions, for example, if the file is not a valid image
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    
    }
}
