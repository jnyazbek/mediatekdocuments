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
    public class RayonTests
    {
        [TestMethod]
        public void Rayon_Test()
        {
            // Arrange
            string expectedId = "0004";
            string expectedLibelle = "Littérature";

            // Act
            Rayon rayon = new Rayon(expectedId, expectedLibelle);

            // Assert
            Assert.AreEqual(expectedId, rayon.Id);
            Assert.AreEqual(expectedLibelle, rayon.Libelle);
        }

        [TestMethod]
        public void Rayon_Libelle_test()
        {
            // Arrange
            string id = "0002";
            string libelle = "Science-Fiction";
            Rayon rayon = new Rayon(id, libelle);

            // Act
            string result = rayon.ToString();

            // Assert
            Assert.AreEqual(libelle, result);
        }
    }
}
