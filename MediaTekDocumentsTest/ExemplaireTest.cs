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
    public class ExemplaireTests
    {
        [TestMethod]
        public void Exemplaire_Constructor_Test()
        {
            // Arrange
            int expectedNumero = 1;
            DateTime expectedDateAchat = new DateTime(2021, 1, 1);
            string expectedPhoto = "photo.jpg";
            string expectedIdEtat = "0001";
            string expectedIdDocument = "10001";

            // Act
            Exemplaire exemplaire = new Exemplaire(expectedNumero, expectedDateAchat, expectedPhoto, expectedIdEtat, expectedIdDocument);

            // Assert
            Assert.AreEqual(expectedNumero, exemplaire.Numero, "Numero doit être = à la valeur attendue");
            Assert.AreEqual(expectedDateAchat, exemplaire.DateAchat, "DateAchat doit être = à la valeur attendue");
            Assert.AreEqual(expectedPhoto, exemplaire.Photo, "Photodoit être = à la valeur attendue");
            Assert.AreEqual(expectedIdEtat, exemplaire.IdEtat, "IdEtat doit être = à la valeur attendue");
            Assert.AreEqual(expectedIdDocument, exemplaire.Id, "Id doit être = à la valeur attendue");
        }
    }
}
