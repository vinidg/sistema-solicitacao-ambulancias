using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solicitacao_de_Ambulancias
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (System.IO.File.Exists("D:\\Sistema de Solicitação de Ambulancias\\Sistema de Solicitação de Ambulancias\\pastaDTI.bat"))
            {
                System.Diagnostics.Process.Start("D:\\Sistema de Solicitação de Ambulancias\\Sistema de Solicitação de Ambulancias\\pastaDTI.bat");
            }
            else
            {
                System.Diagnostics.Process.Start("C:\\Sistema de Solicitação de Ambulancias\\pastaDTI.bat");
            }
            Update updatando = new Update();
            updatando.up();

            if (updatando.Yn == true)
            {
                Environment.Exit(1);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfirmaSolicitacao());
        }
    }
}
