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
    public class SuiviTests
    {
        [TestMethod]
        public void Suivi_Tests()
        {
            // Arrange
            string expectedId = "1";
            string expectedLibelle = "En cours";
            string updatedLibelle = "Terminé";

            // Act
            Suivi suivi = new Suivi(expectedId, expectedLibelle);

            // Assert Initial Values
            Assert.AreEqual(expectedId, suivi.Id);
            Assert.AreEqual(expectedLibelle, suivi.Libelle);

            // Act Update Values
            suivi.Libelle = updatedLibelle;

            // Assert Updated Values
            Assert.AreEqual(expectedId, suivi.Id); // ID should remain unchanged
            Assert.AreEqual(updatedLibelle, suivi.Libelle); // Libelle should update to new value
        }
    }
}
