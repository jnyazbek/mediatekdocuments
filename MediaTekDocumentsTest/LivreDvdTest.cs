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
    public class LivreDvdTests
    {
        [TestMethod]
        public void LivreDvd_Constructor_Test()
        {
            // Arrange - using Livre as a concrete class for testing LivreDvd
            string expectedId = "0001";
            string expectedTitre = "Titre";
            string expectedImage = "exampleimage.jpg";
            string expectedIsbn = "ISBN123456";
            string expectedAuteur = "Jean De la Fontaine";
            string expectedCollection = "Les fables";
            string expectedIdGenre = "0008";
            string expectedGenre = "Contes";
            string expectedIdPublic = "0007";
            string expectedPublic = "Ado";
            string expectedIdRayon = "0004";
            string expectedRayon = "Fantaisie";

            // Act
            Livre livre = new Livre(expectedId, expectedTitre, expectedImage, expectedIsbn, expectedAuteur,
                                    expectedCollection, expectedIdGenre, expectedGenre, expectedIdPublic,
                                    expectedPublic, expectedIdRayon, expectedRayon);

            // Assert - Checking inherited properties from Document
            Assert.AreEqual(expectedId, livre.Id);
            Assert.AreEqual(expectedTitre, livre.Titre);
            Assert.AreEqual(expectedImage, livre.Image);
            Assert.AreEqual(expectedIdGenre, livre.IdGenre);
            Assert.AreEqual(expectedGenre, livre.Genre);
            Assert.AreEqual(expectedIdPublic, livre.IdPublic);
            Assert.AreEqual(expectedPublic, livre.Public);
            Assert.AreEqual(expectedIdRayon, livre.IdRayon);
            Assert.AreEqual(expectedRayon, livre.Rayon);
        }
    }
}
