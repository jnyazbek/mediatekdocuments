using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MediaTekDocumentsTest
{

    [TestClass]
    public class AbonnementTests
    {
        [TestMethod]
        public void Abonnement_Test()
        {
            // Arrange
            string expectedId = "AB123";
            DateTime expectedDateCommande = DateTime.Now;
            double expectedMontant = 99.99;
            DateTime expectedDateFinAbonnement = DateTime.Now.AddYears(1);
            string expectedIdRevue = "RV456";

            // Act
            Abonnement abonnement = new Abonnement(expectedId, expectedDateCommande, expectedMontant, expectedDateFinAbonnement, expectedIdRevue);

            // Assert
            Assert.AreEqual(expectedId, abonnement.Id);
            Assert.AreEqual(expectedDateCommande, abonnement.Date);
            Assert.AreEqual(expectedMontant, abonnement.Montant);
            Assert.AreEqual(expectedDateFinAbonnement, abonnement.DateFinAbonnement);
            Assert.AreEqual(expectedIdRevue, abonnement.IdRevue);
        }

        // Vous pouvez ajouter d'autres méthodes de test pour tester spécifiquement chaque propriété si nécessaire.
    }
}
