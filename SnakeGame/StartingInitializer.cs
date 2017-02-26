using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    static class StartingInitializer
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SettingsForm());
            System.Threading.Thread.CurrentThread.Suspend();
            //Application.Run(new GamingForm());
        }
    }
}
