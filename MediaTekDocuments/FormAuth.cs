using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments
{
    /// <summary>
    /// formulaire d'authentification
    /// </summary>
    public partial class FormAuth : Form
    {
        public int ServiceId { get; private set; }

        private readonly FormAuthController controller;
        
        public FormAuth()
        {
            InitializeComponent();
            controller = new FormAuthController();
            // Si votre TextBox s'appelle textBoxMdp
            textBoxMdp.UseSystemPasswordChar = true;
        }

        private void buttonConnexion_Click(object sender, EventArgs e)
        {
            List<Utilisateur> users = controller.GetAllUtilisateurs();
            Utilisateur utilisateur = null;
            foreach(Utilisateur user in users)
            {
                Console.WriteLine("user " + user.Email + "comaperd to " + textBoxId.Text + "mdp "+ user.Mot_de_passe);
                if(user.Email == textBoxId.Text)
                {
                    utilisateur = user;
                }
            }

            if(utilisateur != null)
            {
                Console.WriteLine("mdp " + textBoxMdp.Text + "compared to "+ utilisateur.Mot_de_passe);
                if (utilisateur.Mot_de_passe == textBoxMdp.Text)
                {
                    this.ServiceId = utilisateur.Id_service; // Set the ServiceId based on the authenticated user
                    Console.WriteLine("formauth the service id is  " + utilisateur.Id_service);
                    this.DialogResult = DialogResult.OK; // Indicate success
                    this.Close(); // Close the form
                     

                }
                else
                {
                    MessageBox.Show("mot de passe incorrect.");
                }
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur inexistant.");
            }

           
        }
    }
}
