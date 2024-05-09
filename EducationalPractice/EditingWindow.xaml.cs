using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EducationalPractice
{
    /// <summary>
    /// Логика взаимодействия для EditingWindow.xaml
    /// </summary>
    public partial class EditingWindow : Window
    {
        public delegate void AddingHandler(User user);
        public event AddingHandler? EditUser;

        private User User;
        public EditingWindow()
        {
            InitializeComponent();
            User = new User
            {
                NameClient = null,
                NameDirector = null
            };
            grid.DataContext = User;
        }
        public EditingWindow(User user)
        {
            InitializeComponent();
            User = new User
            {
                NameClient = null,
                NameDirector = null
            };
            User.CopyValues(user);
            grid.DataContext = User;
            OkButt.Content = "Изменить";
        }

        private void CancelBut(object sender, RoutedEventArgs e) => Close();

        private void AddOrEditButt(object sender, RoutedEventArgs e)
        {
            bool Val = true;
            foreach (var item in grid.Children)
            {
                
                if (item is TextBox tb)
                {
                    tb.AppendText(tb.Text);
                    bool tVal = Validate(tb, null!);
                    Val = Val ? tVal : Val;
                }
            }
            if(Val)
            {
                EditUser?.Invoke(User);
                Close();
            }
        }

        private void Validate(object sender, TextChangedEventArgs e)
        {
            TextBox? tb = sender as TextBox;
            if (tb != null)
            {
                if(string.IsNullOrEmpty(tb.Text))
                {
                    tb.BorderBrush = new SolidColorBrush { Color = Colors.Red };
                    return false;
                }
                else
                {
                    tb.BorderBrush = new SolidColorBrush { Color = Colors.Gray };
                    return true;
                }
            }
            return false;
        }
    }


}
