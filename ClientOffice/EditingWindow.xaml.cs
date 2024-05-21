using Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClientOffice
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
            OkButt.Focus();
            bool Val = true;
            foreach (var item in grid.Children)
            {
                if (item is TextBox tb)
                {
                    if (tb.Tag != null && tb.Tag.ToString() == "Required")
                    {
                        bool tVal = ValidateOnExit(tb);
                        Val = Val ? tVal : Val;
                    }
                }
            }
            if(Val)
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
                if(string.IsNullOrEmpty(tb.Text))
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
    }


}
