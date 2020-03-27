using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WannaSmoke.Client.Forms;
using WannaSmoke.Client.Services.SocketService;

namespace WannaSmoke.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var settings = new Settings(ConfigurationManager.AppSettings);
            var socketService = new SocketService(settings.Config);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(settings, socketService));
        }
        
    }
}
