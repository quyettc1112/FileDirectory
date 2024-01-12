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
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

using FirebaseAdmin;
using System.Net.Sockets;
using System.Threading;
using Firebase.Storage;
using System.DirectoryServices.ActiveDirectory;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

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

        BitmapImage bitmapImage = new BitmapImage();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret= "pruPA0SZKcAoK6ITHBt1GAAla2xo5mQ6Z6qgj2UP",
            BasePath= "https://imagewdf-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;

        public class ImageClasss{ 
            public string ID {  get; set; }
            public string Url { get; set; }
        }

        
        public Picture(string path, string name)
        {
            InitializeComponent();
            Path = path;
            Name = name;  
            
            tb_name.Text = name;
            SetImageSource(path);

         
       
        }

        static async Task UploadImageToFirebaseStorage(string filePath, string Name, BitmapImage bitmapImage)
        {
     
            bitmapImage.StreamSource?.Close();
            bitmapImage = null;


            // Khởi tạo FirebaseStorage và đường dẫn trên Firebase Storage
            FirebaseStorage firebaseStorage = new FirebaseStorage("imagewdf.appspot.com");

                // Đường dẫn trên Firebase Storage
                string firebaseStoragePath = $"images/{Name}";

                 var stream = File.Open(filePath, FileMode.Open);
                
              
                 var task = firebaseStorage
                       .Child(firebaseStoragePath)
                       .PutAsync(stream);
           // stream.Close();
            MessageBox.Show("Upload Success");
            

        }


        private void SetImageSource(string imagePath)
        {
            try
            {
              
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath);

                bitmapImage.EndInit();
                // Assign the BitmapImage to your Image control
                image.Source = bitmapImage;

                bitmapImage.StreamSource?.Close();
                bitmapImage = null;




            }
            catch (Exception ex)
            {
         
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

       
    }
}
