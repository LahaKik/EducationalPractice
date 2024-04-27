using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace EducationalPractice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void CleanCashe(object sender, ExitEventArgs e)
        {
            //string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Cache", "*.png");
            //foreach (string path in files)
            //{
            //    int tryDelete = 0;
            //    while (tryDelete < 100)
            //    {
            //        try
            //        {
            //            File.Delete(path);
            //            break;
            //        }
            //        catch
            //        {
            //            tryDelete++;
            //            Thread.Sleep(1000);
            //        }
            //    }
            //}

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
