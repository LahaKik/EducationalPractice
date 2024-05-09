using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Common;

namespace RemoteOffice
{
    /// <summary>
    /// Логика взаимодействия для EditingWindow.xaml
    /// </summary>
    public partial class EditingWindow : Window
    {
        public delegate void AddingHandler(User user);
        public event AddingHandler? ChangeInUser;

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
            string txt = ResolutionTB.Text;
            ResolutionTB.Text = "";
            ResolutionTB.AppendText(txt);
            bool val = ValidateOnExit(ResolutionTB);
            if (User.Status == Status.Created)
            {
                StatusLabel.Foreground = new SolidColorBrush { Color = Colors.Red };
                return;
            }
            if (val)
            {
                ChangeInUser?.Invoke(User);
                Close();
            }
        }
        #region //Do smth with it 
        private void Validate(object sender, TextChangedEventArgs e)
        {
            TextBox? tb = sender as TextBox;
            if (tb != null)
            {
                if (string.IsNullOrEmpty(tb.Text))
                {
                    tb.BorderBrush = new SolidColorBrush { Color = Colors.Red };
                }
                else
                {
                    tb.BorderBrush = new SolidColorBrush { Color = Colors.Gray };
                }
            }
        }
        private bool ValidateOnExit(object sender)
        {
            TextBox? tb = sender as TextBox;
            if (tb != null)
            {
                if (string.IsNullOrEmpty(tb.Text))
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
        #endregion

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton? button = sender as RadioButton;
            StatusLabel.Foreground = new SolidColorBrush { Color = Colors.Black };
            if (button != null)
            {
                if (button.Content.ToString() == "Рассметрено")
                    User.Status = Status.Reviewed;
                else if (button.Content.ToString() == "Отклонено")
                    User.Status = Status.Rejected;
            }
        }
    }

}

