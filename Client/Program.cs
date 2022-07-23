using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var respond = DialogResult.OK;
            while (respond == DialogResult.OK)
            {
                try
                {
                    SocketManager.socketManager.connectServer();
                    SocketManager.socketManager.ListenThread();
                    break;
                }
                catch
                {
                    respond = MessageBox.Show("Can't connect to server. Do you want to retry?",
                        "Connection failed",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button2);
                    if (respond == DialogResult.Cancel)
                    {
                        SocketManager.socketManager.closeSocket();
                        Application.Exit();
                        return;
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(FormMain.App);

        }

    }
}
