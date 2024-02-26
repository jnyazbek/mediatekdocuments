using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            return access.GetExemplairesRevue(idDocument);
        }
        /// <summary>
        /// récupère les commandes d'un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
       public List<CommandeDocument> GetCommandesDocuments(string idDocument)
        {
            return access.GetCommandesDocument(idDocument);
        }
        /// <summary>
        /// récupère les commandes de dvd
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandesDVD(string idDocument)
        {
            return access.GetCommandesDVD(idDocument);
        }
        /// <summary>
        /// récupère les commandes de revues
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<Abonnement> GetCommandesRevue(string idDocument)
        {
            return access.GetCommandesRevue(idDocument);
        }
        /// <summary>
        /// récupère les suivis
        /// </summary>
        /// <returns></returns>
        public List<Suivi> GetAllSuivi()
        {
            return access.GetAllSuivis();
        }
        /// <summary>
        /// récupère les commandes
        /// </summary>
        /// <returns></returns>
        public List<Commande> GetAllCommandes()
        {
            return access.GetAllCommandes();
        }
        /// <summary>
        /// récupère les commandes document
        /// </summary>
        /// <returns></returns>
        public List<CommandeDocument> GetAllCommandesDocument()
        {
            return access.GetAllCommandesDocument();
        }

        /// <summary>
        /// récupère les commandes de revue
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAllCommandesRevue()
        {
            return access.GetAllCommandesRevue();
        }

        /// <summary>
        /// ajoute une commande de document
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public bool AddCommandeDocument(CommandeDocument commande)
        {
            return access.AddCommandeDocument(commande);
        }
        /// <summary>
        /// ajoute une commande de revue
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public bool AddCommandeRevue(Abonnement commande)
        {
            return access.AddCommandeRevue(commande);
        }
        /// <summary>
        /// ajoute un suivi
        /// </summary>
        /// <param name="suivi"></param>
        /// <returns></returns>
        public bool AddSuivi(Suivi suivi) 
        {
            return access.AddSuivi(suivi);
        }
        /// <summary>
        /// change l'étape de suivi
        /// </summary>
        /// <param name="numSuivi"></param>
        /// <param name="libelle"></param>
        /// <returns></returns>
        public bool ChangeSuivi(string numSuivi, string libelle)
        {
            return access.ChangeSuivi(numSuivi, libelle);
        }
        /// <summary>
        ///supprime une commande de document
        /// </summary>
        /// <param name="commandeDocument"></param>
        /// <returns></returns>
        public bool DeleteCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.DeleteCommandeDocument(commandeDocument);
        }
        /// <summary>
        /// supprime une commande de revue
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public bool DeleteCommandeRevue(Abonnement commande)
        {
            return access.DeleteCommandeRevue(commande);
        }
    }
}