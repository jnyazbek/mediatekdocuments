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
    public class PublicTests
    {
        [TestMethod]
        public void Public_Test()
        {
            // Arrange
            string expectedId = "00017";
            string expectedLibelle = "Jeunesse";

            // Act
            Public publicCible = new Public(expectedId, expectedLibelle);

            // Assert
            Assert.AreEqual(expectedId, publicCible.Id);
            Assert.AreEqual(expectedLibelle, publicCible.Libelle);
        }

        [TestMethod]
        public void Public_Libelle_Test()
        {
            // Arrange
            string id = "00018";
            string libelle = "Adultes";
            Public publicCible = new Public(id, libelle);

            // Act
            string result = publicCible.ToString();

            // Assert
            Assert.AreEqual(libelle, result);
        }
    }
}
