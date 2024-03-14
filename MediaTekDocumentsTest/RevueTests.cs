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
    public class RevueTests
    {
        [TestMethod]
        public void Revue_Test()
        {
            // Arrange
            string expectedId = "0009";
            string expectedTitre = "National Geographic";
            string expectedImage = "natgeo.png";
            string expectedIdGenre = "0007";
            string expectedGenre = "Science";
            string expectedIdPublic = "0009";
            string expectedPublic = "Adultes";
            string expectedIdRayon = "0005";
            string expectedRayon = "Nature et Découverte";
            string expectedPeriodicite = "Mensuelle";
            int expectedDelaiMiseADispo = 30;

            // Act
            Revue revue = new Revue(expectedId, expectedTitre, expectedImage, expectedIdGenre, expectedGenre,
                expectedIdPublic, expectedPublic, expectedIdRayon, expectedRayon, expectedPeriodicite, expectedDelaiMiseADispo);

            // Assert
            Assert.AreEqual(expectedId, revue.Id);
            Assert.AreEqual(expectedTitre, revue.Titre);
            Assert.AreEqual(expectedImage, revue.Image);
            Assert.AreEqual(expectedIdGenre, revue.IdGenre);
            Assert.AreEqual(expectedGenre, revue.Genre);
            Assert.AreEqual(expectedIdPublic, revue.IdPublic);
            Assert.AreEqual(expectedPublic, revue.Public);
            Assert.AreEqual(expectedIdRayon, revue.IdRayon);
            Assert.AreEqual(expectedRayon, revue.Rayon);
            Assert.AreEqual(expectedPeriodicite, revue.Periodicite);
            Assert.AreEqual(expectedDelaiMiseADispo, revue.DelaiMiseADispo);
        }
    }
}
