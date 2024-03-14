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
    public class DocumentTests
    {
        [TestMethod]
        public void Document_Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            string id = "0008";
            string titre = "Titre Document";
            string image = "image.jpg";
            string idGenre = "0001";
            string genre = "Roman";
            string idPublic = "00078";
            string lePublic = "Adulte";
            string idRayon = "0007";
            string rayon = "Littérature";

            // Act
            Document document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);

            // Assert
            Assert.AreEqual(id, document.Id, "Id doit être = à la valeur attendue");
            Assert.AreEqual(titre, document.Titre, "Titre doit être = à la valeur attendue");
            Assert.AreEqual(image, document.Image, "Image doit être = à la valeur attendue");
            Assert.AreEqual(idGenre, document.IdGenre, "IdGenre doit être = à la valeur attendue");
            Assert.AreEqual(genre, document.Genre, "Genre doit être = à la valeur attendue");
            Assert.AreEqual(idPublic, document.IdPublic, "IdPublic doit être = à la valeur attendue");
            Assert.AreEqual(lePublic, document.Public, "Public doit être = à la valeur attendue");
            Assert.AreEqual(idRayon, document.IdRayon, "IdRayon doit être = à la valeur attendue");
            Assert.AreEqual(rayon, document.Rayon, "Rayon doit être = à la valeur attendue");
        }
    }
}
