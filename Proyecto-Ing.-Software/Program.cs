using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Ing._Software
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var login = new FormLogin())
            {
                var result = login.ShowDialog();
                if (result != DialogResult.OK) return; 
                                                       
            }
            Application.Run(new FormMenuPrincipal());
        }
    }
}
