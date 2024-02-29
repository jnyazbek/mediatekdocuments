using MediaTekDocuments.view;
using System;
using System.Windows.Forms;

namespace MediaTekDocuments
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // First, show the login form
            FormAuth loginForm = new FormAuth();
            DialogResult result = loginForm.ShowDialog();

            // Check the result
            if (result == DialogResult.OK)
            {
                Console.WriteLine(" main id is " + loginForm.ServiceId);
                // If login was successful, use the ServiceId from FormAuth
                Application.Run(new FrmMediatek(loginForm.ServiceId));
            }
            else
            {
                Application.Exit(); // Exit the application if login was not successful or was canceled
            }
        }
    }
}
