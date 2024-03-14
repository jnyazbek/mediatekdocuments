using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{/// <summary>
/// Classe métier permettant d'identifier les different utilisateurs de l'application
/// </summary>
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Mot_de_passe { get; set; } // À utiliser avec prudence
        public int Id_service { get; set; } // Clé étrangère vers la table Service
    }

}
