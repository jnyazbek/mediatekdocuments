using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class UtilisateurTests
    {
        [TestMethod]
        public void Utilisateur_Test()
        {
            // Arrange
            int initialId = 1;
            string initialNom = "Dupont";
            string initialPrenom = "Jean";
            string initialEmail = "jean.dupont@example.com";
            string initialMotDePasse = "password123";
            int initialIdService = 2;

            // Act - Création de l'utilisateur
            Utilisateur utilisateur = new Utilisateur
            {
                Id = initialId,
                Nom = initialNom,
                Prenom = initialPrenom,
                Email = initialEmail,
                Mot_de_passe = initialMotDePasse,
                Id_service = initialIdService
            };

            // Assert - Vérification des valeurs initiales
            Assert.AreEqual(initialId, utilisateur.Id);
            Assert.AreEqual(initialNom, utilisateur.Nom);
            Assert.AreEqual(initialPrenom, utilisateur.Prenom);
            Assert.AreEqual(initialEmail, utilisateur.Email);
            Assert.AreEqual(initialMotDePasse, utilisateur.Mot_de_passe);
            Assert.AreEqual(initialIdService, utilisateur.Id_service);

            // Act - Modification des valeurs
            string updatedNom = "Martin";
            utilisateur.Nom = updatedNom;
            int updatedIdService = 3;
            utilisateur.Id_service = updatedIdService;

            // Assert - Vérification des valeurs modifiées
            Assert.AreEqual(updatedNom, utilisateur.Nom);
            Assert.AreEqual(updatedIdService, utilisateur.Id_service);
        }
    }
}
