﻿using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Common;

namespace ClientOffice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void CleanCaсhe(object sender, EventArgs e)
        {
            int tryDelete = 0;
            while (tryDelete < 100)
            {
                try
                {
                    Directory.Delete(Environment.CurrentDirectory + "\\Cache", true);
                    break;
                }
                catch(Exception ex)
                {
                    if (ex is DirectoryNotFoundException)
                        break;
                    tryDelete++;
                    Thread.Sleep(100);
                }
            }
        }
    }
}
