using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ZXing.QrCode.Internal.Mode;

namespace EducationalPractice
{
    /// <summary>
    /// Логика взаимодействия для EditingWindow.xaml
    /// </summary>
    public partial class EditingWindow : Window
    {
        public delegate void AddingHandler(User user);
        public event AddingHandler? EditUser;
        int? Iduser;
        public EditingWindow()
        {
            InitializeComponent();
        }
        public EditingWindow(User user)
        {
            InitializeComponent();

            Iduser = user.Id;
            NameClientTB.Text = user.NameClient;
            NameDirTB.Text = user.NameDirector;
            AddressTB.Text = user.Address;
            ThemeTB.Text = user.Theme;
            ContentTB.Text = user.Content;
            ResolutionTB.Text = user.Resolution;
            NoteTB.Text = user.Note;
            OkButt.Content = "Изменить";
        }

        private void CancelBut(object sender, RoutedEventArgs e) => Close();

        private void AddOrEditButt(object sender, RoutedEventArgs e)
        {
            EditUser?.Invoke(new User
            {
                Id = Iduser ?? null,
                NameClient = NameClientTB.Text,
                NameDirector = NameDirTB.Text,
                Address = AddressTB.Text,
                Theme = ThemeTB.Text,
                Content = ContentTB.Text,
                Resolution = ResolutionTB.Text,
                Note = NoteTB.Text,
                Status = Status.Created
            });
            Close();
        }
    }
}
