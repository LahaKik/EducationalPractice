using System.Windows;

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
