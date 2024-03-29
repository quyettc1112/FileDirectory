﻿using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
using Winforms = System.Windows.Forms;
using static FileDirectory.MainWindow;

namespace FileDirectory
{
    /// <summary>
    /// Interaction logic for Camera.xaml
    /// </summary>
    public partial class Camera : Window
    {


        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private Bitmap currentFrame;


        public ObservableCollection<MyData> DataList { get; set; }
        public string Path { get; set; } 
    
        public Camera()
        {
            InitializeComponent();
            Loaded += Camera_Loaded;
            Closing += Camera_Closing;
            DataList = new ObservableCollection<MyData>
            {
            };


        }

        private void Camera_Loaded(object sender, RoutedEventArgs e)
        {
            try {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count > 0)
                {
                    videoSource = new VideoCaptureDevice((videoDevices[0].MonikerString));
                    videoSource.NewFrame += VideoSource_NewFrame;
                    videoSource.Start();
                }
                else
                {
                    System.Windows.MessageBox.Show("No video devices found.");
                }

            } catch (Exception ex)
            {

            }

            
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try {
                ig_camera.Dispatcher.Invoke(() =>
                {
                    ig_camera.Source = ToBitmapImage(eventArgs.Frame);
                    currentFrame = eventArgs.Frame.Clone() as Bitmap;

                });
            }
            catch (Exception ex) { }

           
        }


        private System.Windows.Media.Imaging.BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;

                System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }


        private void Camera_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
              // videoSource.Stop();

            }
        }
        public class MyData
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string Icon { get; set; }
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
                    Icon = "C:\\Users\\Admin\\OneDrive\\Desktop\\PRN221\\FileDirectory\\vector-folder-icon.jpg"
                });
            };

            foreach (string file in files)
            {
                string[] result = file.Split('\\');
                DataList.Add(new MyData
                {
                    Name = $"{result[result.Length - 1]}",
                    Path = $"{file}",
                    Icon = "C:\\Users\\Admin\\OneDrive\\Desktop\\PRN221\\FileDirectory\\filesIcon.jfif"
                });
            };
            lv_listfolder.ItemsSource = DataList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] pathArray = Path.Split('\\');
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
                Path = result;
                tb_folderpath.Text = Path;
                ReadFileorFolder(Path);
            }
        }

        private void btn_browser_Click(object sender, RoutedEventArgs e)
        {
            tb_folderpath.Text = "";

            DataList = new ObservableCollection<MyData>
            {
            };
            DataList.Clear();

            Winforms.FolderBrowserDialog dialog = new Winforms.FolderBrowserDialog();
            Winforms.DialogResult dialogResult = dialog.ShowDialog();

            if (dialogResult == Winforms.DialogResult.OK)
            {
                Path = dialog.SelectedPath;
                tb_folderpath.Text = Path;
                try
                {
                    ReadFileorFolder(Path);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Cancel select file", "Cancel");
                DataList.Clear();
                lv_listfolder.ItemsSource = DataList;
                tb_folderpath.Text = "";
            }
        }

        private void lv_listfolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = lv_listfolder.SelectedIndex;
            string pathToOpen = DataList[selectedIndex].Path;


            tb_folderpath.Text = pathToOpen;
            Path = pathToOpen;
            if (IsDirectory(Path) == true)
            {
                ReadFileorFolder(Path);
            }

            if (IsImageFile(Path))
            {
                Picture pt = new Picture(Path, DataList[selectedIndex].Name);
                pt.Show();
            }
        }

        private void btn_capture_Click(object sender, RoutedEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {

                if (IsImageFile(Path))
                {
                    string[] pathArray = Path.Split('\\');
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
                        Path = result;
                        tb_folderpath.Text = Path;
                    }
                }
                else {
                    if (!string.IsNullOrWhiteSpace(Path) &&
                        !string.IsNullOrWhiteSpace(tb_folderpath.Text))
                    {
                        // Tạo đường dẫn lưu ảnh
                        string fileName = $"snapshot_{DateTime.Now:yyyyMMddHHmmss}.png";
                        string filePath = System.IO.Path.Combine(Path, fileName);

                        currentFrame.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                        System.Windows.MessageBox.Show($"Snapshot saved to {filePath}");
                        ReadFileorFolder(Path);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Please select a folder before capturing a snapshot.");
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Webcam is not running.");
            }
        }

        private void tb_folderpath_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
