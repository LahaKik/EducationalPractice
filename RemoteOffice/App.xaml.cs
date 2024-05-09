using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace RemoteOffice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void CleanCashe(object sender, ExitEventArgs e)
        {
            int tryDelete = 0;
            while (tryDelete < 100)
            {
                try
                {
                    Directory.Delete(Environment.CurrentDirectory + "\\Cache", true);
                    break;
                }
                catch
                {
                    tryDelete++;
                    Thread.Sleep(100);
                }
            }
        }
    }

}
