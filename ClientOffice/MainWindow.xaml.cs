using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.IO;
using Common;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;

namespace ClientOffice
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

        string CachePath = Environment.CurrentDirectory + @"\Cache";

        UsersDB db = new UsersDB();
        User? UserQR;
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(CachePath);
            Loaded += MainWindow_Loaded;
            QRCreated += ApplyImage;
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            db.Users.Load();
            DataContext = db.Users.Local.ToObservableCollection();
        }
        private async void LoButt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Выберите путь для сохранения",
                Filter = "PNG-изображение|*.png",
                
                DefaultExt = ".png"
            };
            string filename = "";
            if (fileDialog.ShowDialog() == true 
                && DataContext is ObservableCollection<User> obsCollDC)
            {
                filename = fileDialog.FileName;
                User? LoadUser = QRCoder.ReadQR(filename);
                if(LoadUser != null )
                {
                    User? dbUser = obsCollDC.FirstOrDefault(user => user.Id == LoadUser.Id);
                    if( dbUser != null )
                    {
                        ConfirmationWindow confWindow = new ConfirmationWindow(dbUser, LoadUser);
                        confWindow.Owner = this;
                        confWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        BlurForGrid.Radius = 10;
                        if (confWindow.ShowDialog() == true)
                        {
                            BlurForGrid.Radius = 0;
                            dbUser.CopyValues(LoadUser);
                        }
                        else
                        {
                            BlurForGrid.Radius = 0;
                            return;
                        }
                    }
                    else
                    {
                        Add_User(LoadUser);
                        dbUser = obsCollDC.FirstOrDefault(user => user.Id == LoadUser.Id);
                    }
                    UserQR = dbUser;
                    LabelQR.Content = "QR код пользователя с Id:" + dbUser!.Id;
                    if (SaveButt.IsEnabled == false)
                        SaveButt.IsEnabled = true;

                    string newPath = CachePath + @$"\QR{NQRCashe}.png";
                    File.Copy(filename, newPath);
                    dbUser!.QRPath = newPath;
                    ApplyImage(newPath);
                    ListOfNotes.Items.Refresh();
                    await db.SaveChangesAsync();
                }
                else
                {
                    MessageBox.Show("Файл не содержит QR-кода или поврежден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Add_CLK(object sender, RoutedEventArgs e)
        {
            EditingWindow window = new EditingWindow();
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ChangeInUser += Add_User;
            Blur(window);
        }

        private void Edit_CLK(object sender, RoutedEventArgs e)
        {
            User? user = ListOfNotes.SelectedItem as User;
            if (user != null)
            {
                EditingWindow window = new EditingWindow(user);
                window.Owner = this;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.ChangeInUser += Edit_User;
                Blur(window);
            }
        }
        private async void Del_CLK(object sender, RoutedEventArgs e)
        {        
            User? user = ListOfNotes.SelectedItem as User;
            if (user != null
                && DataContext is ObservableCollection<User> obsCollDC)
            {
                obsCollDC.Remove(user);
                ListOfNotes.Items.Refresh();
                await db.SaveChangesAsync();
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
                        string path = CachePath + @$"\QR{NQRCashe}.png";
                        user.QRPath = path;
                        bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        QRCreated?.Invoke(path);
                    }
                    else
                    {
                        QRCreated?.Invoke(user.QRPath);
                    }
                    UserQR = user;
                    LabelQR.Content = "QR код пользователя с Id:" + user.Id;
                    if (SaveButt.IsEnabled == false)
                        SaveButt.IsEnabled = true;
                }
            }
        }
        private void ApplyImage(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = stream;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();
                img.Source = bmi;
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
                Filter = "PNG-изображение|*.png|Документ PDF|*.pdf",
                DefaultExt = ".png",
            };

            if (fileDialog.ShowDialog() == true)
            {
                string filename = fileDialog.FileName;
                string ext = Path.GetExtension(filename);
                if (ext == ".png")
                    File.Copy(UserQR.QRPath, filename, true);
                else if (ext == ".pdf")
                    PDFWriter.GeneratePDF(filename, UserQR.QRPath, UserQR.ToStringArray());
                else
                    MessageBox.Show("Выберите расширение из списка", "Неправльный формат файла!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Файл не сохранен", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void Add_User(User user)
        {
            if(DataContext is ObservableCollection<User> obsCollDC)
            {
                obsCollDC.Add(user);
                await db.SaveChangesAsync();
            }
        }
        private async void Edit_User(User EdUser)
        {
            if (DataContext is ObservableCollection<User> obsCollDC)
            {
                User? editedUser = obsCollDC.FirstOrDefault(u => u.Id == EdUser.Id);
                editedUser?.CopyValues(EdUser);
                ListOfNotes.Items.Refresh();
                await db.SaveChangesAsync();
            }
        }
        private void Blur(Window window)
        {
            BlurForGrid.Radius = 10;
            if (window.ShowDialog() != null)
            {
                BlurForGrid.Radius = 0;
            }
        }

        private void StartAnimate(object sender, MouseEventArgs e)
        {
            DoubleAnimation buttonAnimWidth = new DoubleAnimation();
            DoubleAnimation buttonAnimHeight = new DoubleAnimation();
            buttonAnimHeight.From = HelpButton.ActualHeight;
            buttonAnimWidth.From = HelpButton.ActualWidth;
            buttonAnimHeight.To = 50;
            buttonAnimWidth.To = 350;
            buttonAnimHeight.Duration = TimeSpan.FromSeconds(0.5);
            buttonAnimWidth.Duration = TimeSpan.FromSeconds(0.5);
            HelpButton.Content =
                "Двойной клик по строчке таблицы - создать QR код\n" +
                "ПКМ по выбранной строчке - выбрать действие с заявлением";
            HelpButton.BeginAnimation(HeightProperty, buttonAnimHeight);
            HelpButton.BeginAnimation(WidthProperty, buttonAnimWidth);
        }

        private void StopAnimate(object sender, MouseEventArgs e)
        {

            DoubleAnimation buttonAnimWidth = new DoubleAnimation();
            DoubleAnimation buttonAnimHeight = new DoubleAnimation();
            buttonAnimHeight.From = HelpButton.ActualHeight;
            buttonAnimWidth.From = HelpButton.ActualWidth;
            buttonAnimHeight.To = 20;
            buttonAnimWidth.To = 20;
            buttonAnimHeight.Duration = TimeSpan.FromSeconds(0.5);
            buttonAnimWidth.Duration = TimeSpan.FromSeconds(0.5);
            HelpButton.Content = "?";
            HelpButton.BeginAnimation(HeightProperty, buttonAnimHeight);
            HelpButton.BeginAnimation(WidthProperty, buttonAnimWidth);
        }
        private async void DropButt_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Данное действие удалит все данные в локальной базе данных!", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK
                    || MessageBox.Show("Вы точно уверены?", "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            if (DataContext is ObservableCollection<User> obsCollDC)
            {
                obsCollDC.Clear();
            }
            await db.SaveChangesAsync();
        }

        private void Save(object sender, System.ComponentModel.CancelEventArgs e) => db.SaveChanges();
    }
}