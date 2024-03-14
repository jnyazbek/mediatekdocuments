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
    public class DvdTests
    {
        [TestMethod]
        public void Dvd_Constructor_ShouldInitializeAllPropertiesCorrectly()
        {
            // Arrange
            string id = "0008";
            string titre = "La momie";
            string image = "dvd_image.jpg";
            int duree = 120; // en minutes
            string realisateur = "Jean Dupont";
            string synopsis = "Ceci est un synopsis du DVD.";
            string idGenre = "0002";
            string genre = "Action";
            string idPublic = "0009";
            string lePublic = "Tout public";
            string idRayon = "0003";
            string rayon = "Multimédia";

            // Act
            Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);

            // Assert
            Assert.AreEqual(id, dvd.Id, "Id doit être = à la valeur attendue");
            Assert.AreEqual(titre, dvd.Titre, "Titre doit être = à la valeur attendue");
            Assert.AreEqual(image, dvd.Image, "Image doit être = à la valeur attendue");
            Assert.AreEqual(duree, dvd.Duree, "Duree doit être = à la valeur attendue");
            Assert.AreEqual(realisateur, dvd.Realisateur, "Realisateur doit être = à la valeur attendue");
            Assert.AreEqual(synopsis, dvd.Synopsis, "Synopsis doit être = à la valeur attendue");
            // Also asserting the inherited properties
            Assert.AreEqual(idGenre, dvd.IdGenre, "IdGenre doit être = à la valeur attendue");
            Assert.AreEqual(genre, dvd.Genre, "Genre doit être = à la valeur attendue");
            Assert.AreEqual(idPublic, dvd.IdPublic, "IdPublicdoit être = à la valeur attendue");
            Assert.AreEqual(lePublic, dvd.Public, "Public doit être = à la valeur attendue");
            Assert.AreEqual(idRayon, dvd.IdRayon, "IdRayon doit être = à la valeur attendue");
            Assert.AreEqual(rayon, dvd.Rayon, "Rayon doit être = à la valeur attendue");
        }
    }
}
