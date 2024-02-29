using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller
{
    class FormAuthController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FormAuthController()
        {
            access = Access.GetInstance();
        }

        public List<Utilisateur> GetUtilisateur(string id)
        {
           return  access.GetUtilisateur(id);
            

        }
        public List<Utilisateur> GetAllUtilisateurs()
        {
            return access.GetAllUtilisateurs();

        }
    }
}
