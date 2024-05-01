using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.IO;

namespace EducationalPractice
{
    public partial class MainWindow : Window
    {
        private static int _nqrcashe = 1;
        private static int NQRCashe 
        {
            get
            {
                return _nqrcashe++;
            }
        }

        delegate void QRHandler(string path);
        event QRHandler QRCreated;        


        UsersDB db = new UsersDB();
        User? UserQR;
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\Cache");
            Loaded += MainWindow_Loaded;
            QRCreated += ApplyImage;
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            db.Users.Load();
            DataContext = db.Users.Local.ToObservableCollection();
            
        }

        private void LoButt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Выберите путь для сохранения",
                Filter = "PNG-изображение|*.png",
                
                DefaultExt = ".png"
            };
            string filename = "";
            if (fileDialog.ShowDialog() == true)
            {
                filename = fileDialog.FileName;
                img.Source = BitmapFrame.Create(new Uri(filename));
            }
            else
            {
                MessageBox.Show("Файл не загружен", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Add_CLK(object sender, RoutedEventArgs e)
        {
            EditingWindow window = new EditingWindow();
            window.Owner = this;
            window.Clip = this.VisualClip;
            window.EditUser += Add_User;
            BlurForGrid.Radius = 10;
            if (window.ShowDialog() != null)
            {
                BlurForGrid.Radius = 0;
            }
        }

        private void Edit_CLK(object sender, RoutedEventArgs e)
        {
            User? user = ListOfNotes.SelectedItem as User;
            if (user != null)
            {
                EditingWindow window = new EditingWindow(user);
                window.Owner = this;
                window.EditUser += Edit_User;
                BlurForGrid.Radius = 10;
                if (window.ShowDialog() != null)
                {
                    BlurForGrid.Radius = 0;
                }
            }
        }
        private void Del_CLK(object sender, RoutedEventArgs e)
        {        
            User? user = ListOfNotes.SelectedItem as User;
            if (user != null)
            {
                db.Remove(user);
                db.SaveChanges();
                DataContext = db.Users.Local.ToObservableCollection();
                ListOfNotes.Items.Refresh();
                
            }
        }

        private void CreateQR_DCLK(object sender, MouseButtonEventArgs e)
        {
            DataGrid? dataGrid = sender as DataGrid;
            if(dataGrid != null)
            {
                User? user = dataGrid.SelectedItem as User;
                if(user != null)
                {
                    if(user.QRPath == null)
                    {
                        string UserString = JsonSerializer.Serialize<User>(user);
                        Bitmap bitmap = QRCoder.CreateQRBitmap(UserString);
                        string path = Environment.CurrentDirectory + "\\Cache\\" + "QR" + NQRCashe.ToString() + ".png";
                        user.QRPath = path;
                        bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        QRCreated?.Invoke(path);
                    }
                    else
                    {
                        QRCreated?.Invoke(user.QRPath);
                    }
                    UserQR = user;
                }
            }
        }


        private void ApplyImage(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = stream;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                img.Source = bmp;
            }
        }

        private void SaveButt_Click(object sender, RoutedEventArgs e)
        {
            if (UserQR == null || UserQR.QRPath == null)
                return;

            SaveFileDialog fileDialog = new SaveFileDialog
            {
                Title = "Выберите путь для сохранения",
                OverwritePrompt = true,
                Filter = "PNG-изображение|*.png",
                DefaultExt = ".png",

            };

            string filename = "";
            if (fileDialog.ShowDialog() == true)
            {
                filename = fileDialog.FileName;
                File.Copy(UserQR.QRPath, filename, true);
            }
            else
            {
                MessageBox.Show("Файл не сохранен", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Add_User(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            DataContext = db.Users.Local.ToObservableCollection();
        }
        private void Edit_User(User EdUser)
        {
            User? editedUser = db.Users.FirstOrDefault(u => u.Id == EdUser.Id);
            editedUser?.CopyValues(EdUser);
            db.SaveChanges();
            DataContext = db.Users.Local.ToObservableCollection();
            ListOfNotes.Items.Refresh();
        }

        //private void DelCanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    User? user = ListOfNotes.SelectedItem as User;
        //    if (user != null)
        //    {
        //        e.CanExecute = true;
        //    }
        //    else
        //        e.CanExecute = false;
        //}
    }
}