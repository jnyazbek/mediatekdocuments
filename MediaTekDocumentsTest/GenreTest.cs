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
    public class GenreTests
    {
        [TestMethod]
        public void Genre_Constructor_Test()
        {
            // Arrange
            string expectedId = "0003";
            string expectedLibelle = "Science-Fiction";

            // Act
            Genre genre = new Genre(expectedId, expectedLibelle);

            // Assert
            Assert.AreEqual(expectedId, genre.Id, "Id doit être = à la valeur attendue.");
            Assert.AreEqual(expectedLibelle, genre.Libelle, "Libelle doit être = à la valeur attendue");
        }
    }
}
