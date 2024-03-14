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
    public class CommandeDocumentTests
    {
        [TestMethod]
        public void CommandeDocument_Test()
        {
            // Arrange
            string id = "0008";
            DateTime dateCommande = DateTime.Now;
            double montant = 150.0;
            int nbExemplaire = 3;
            string idLivreDvd = "0008";
            string idSuivi = "0008";
            string libelle = "Livrée";

            // Act
            CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, libelle);

            // Assert
            Assert.AreEqual(id, commandeDocument.Id, "Id doit être = à la valeur attendue");
            Assert.AreEqual(dateCommande, commandeDocument.Date, "DateCommande doit être = à la valeur attendue");
            Assert.AreEqual(montant, commandeDocument.Montant, "Montantdoit être = à la valeur attendue");
            Assert.AreEqual(nbExemplaire, commandeDocument.NbExemplaire, "NbExemplaire doit être = à la valeur attendue");
            Assert.AreEqual(idLivreDvd, commandeDocument.IdLivreDvd, "IdLivreDvd doit être = à la valeur attendue");
            Assert.AreEqual(idSuivi, commandeDocument.Suivi.Id, "IdSuivi doit être = à la valeur attendue");
            Assert.AreEqual(libelle, commandeDocument.LibelleSuivi, "LibelleSuivi doit être = à la valeur attendue");
        }
    }
}
