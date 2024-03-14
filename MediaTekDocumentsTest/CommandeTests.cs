using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;


namespace MediaTekDocumentsTest
{
    [TestClass]
    public class CommandeTests
    {
        [TestMethod]
        public void Commande_Test()
        {
            // Arrange
            string id = "0008";
            DateTime date = DateTime.Now;
            double montant = 100.0;
            string idSuivi = "0008";
            string libelle = "En cours";

            // Act
            Commande commande = new Commande(id, date, montant, idSuivi, libelle);

            // Assert
            Assert.AreEqual(id, commande.Id, "Id doit être = à la valeur attendue");
            Assert.AreEqual(date, commande.Date, "Date doit être = à la valeur attendue");
            Assert.AreEqual(montant, commande.Montant, "Montant doit être = à la valeur attendue");
            Assert.IsNotNull(commande.Suivi, "Suivi doit être = à la valeur attendue.");
            Assert.AreEqual(idSuivi, commande.Suivi.Id, "Suivi Id doit être = à la valeur attendue");
            Assert.AreEqual(libelle, commande.Suivi.Libelle, "Suivi Libelle doit être = à la valeur attendue");
        }

        [TestMethod]
        public void LibelleSuivi_LibelleIfSuiviExists()
        {
            // Arrange
            Commande commande = new Commande("0008", DateTime.Now, 100.0, "0008", "En cours");

            // Act
            string actualLibelle = commande.LibelleSuivi;

            // Assert
            Assert.AreEqual("En cours", actualLibelle, "LibelleSuivi doit être = à la valeur attendue");
        }

        [TestMethod]
        public void LibelleSuivi_EmptyStringIfSuiviIsNull()
        {
            // Arrange
            Commande commande = new Commande("0008", DateTime.Now, 100.0, null, null)
            {
                Suivi = null // Explicitly setting Suivi to null for this test.
            };

            // Act
            string actualLibelle = commande.LibelleSuivi;

            // Assert
            Assert.AreEqual("", actualLibelle, "LibelleSuivi doit être = à la valeur attendue");
        }
    }
}
