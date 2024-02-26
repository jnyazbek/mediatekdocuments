using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Commande
    {
        /// <summary>
        /// Récupère ou définit l'id de la commande
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Récupère ou définit la date de la commande
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Récupère ou définit le montant de la commande
        /// </summary>
        public double Montant { get; set; }

        public Suivi Suivi { get; set; }

        public string Libelle { get; set; }

        public string LibelleSuivi
        {
            get { return Suivi != null ? Suivi.Libelle : ""; }
        }

        /// <summary>
        /// Initialisation d'un nouvel objet Commande
        /// </summary>
        /// <param name="id">Id de la commande</param>
        /// <param name="date">Date de la commande</param>
        /// <param name="montant">Montant de la commande</param>
        /// <param name="libelle">suivi de la commande</param>
        public Commande(string id, DateTime date, double montant,string idSuivi, string libelle)
        {
            this.Id = id;
            this.Date = date;
            this.Montant = montant;
            this.Suivi = new Suivi(idSuivi,libelle);
        }
    }
}
