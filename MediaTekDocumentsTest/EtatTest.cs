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
    public class EtatTests
    {
        [TestMethod]
        public void Etat_Test()
        {
            // Arrange
            string expectedId = "0001";
            string expectedLibelle = "Neuf";

            // Act
            Etat etat = new Etat(expectedId, expectedLibelle);

            // Assert
            Assert.AreEqual(expectedId, etat.Id, "Id doit être = à la valeur attendue");
            Assert.AreEqual(expectedLibelle, etat.Libelle, "Libelle doit être = à la valeur attendue");
        }
    }
}

