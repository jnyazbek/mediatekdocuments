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
    public class LivreTests
    {
        [TestMethod]
        public void Livre_Constructor_Test()
        {
            // Arrange
            string expectedId = "0001";
            string expectedTitre = "Titre";
            string expectedImage = "couverture.jpg";
            string expectedIsbn = "123A4567890123";
            string expectedAuteur = "John Doe";
            string expectedCollection = "Collection";
            string expectedIdGenre = "0007";
            string expectedGenre = "Fiction";
            string expectedIdPublic = "0008";
            string expectedPublic = "Adultes";
            string expectedIdRayon = "0009";
            string expectedRayon = "General";

            // Act
            Livre livre = new Livre(expectedId, expectedTitre, expectedImage, expectedIsbn, expectedAuteur,
                                    expectedCollection, expectedIdGenre, expectedGenre, expectedIdPublic,
                                    expectedPublic, expectedIdRayon, expectedRayon);

            // Assert
            Assert.AreEqual(expectedId, livre.Id);
            Assert.AreEqual(expectedTitre, livre.Titre);
            Assert.AreEqual(expectedImage, livre.Image);
            Assert.AreEqual(expectedIsbn, livre.Isbn);
            Assert.AreEqual(expectedAuteur, livre.Auteur);
            Assert.AreEqual(expectedCollection, livre.Collection);
            Assert.AreEqual(expectedIdGenre, livre.IdGenre);
            Assert.AreEqual(expectedGenre, livre.Genre);
            Assert.AreEqual(expectedIdPublic, livre.IdPublic);
            Assert.AreEqual(expectedPublic, livre.Public);
            Assert.AreEqual(expectedIdRayon, livre.IdRayon);
            Assert.AreEqual(expectedRayon, livre.Rayon);
        }
    }
}
