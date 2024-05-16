using Common;
using System.Windows;

namespace RemoteOffice
{
    /// <summary>
    /// Логика взаимодействия для ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public ConfirmationWindow(User oldUser, User newUser)
        {
            InitializeComponent();
            oldTB.Text = oldUser.ToString();
            newTB.Text = newUser.ToString();
        }

        private void CancerCLK(object sender, RoutedEventArgs e) => Close();

        private void ConfirmCLK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
