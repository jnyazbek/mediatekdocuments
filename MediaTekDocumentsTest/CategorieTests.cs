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
    public class CategorieTests
    {
        [TestMethod]
        public void Categorie_Test()
        {
            // Arrange
            string expectedId = "0008";
            string expectedLibelle = "Fiction";

            // Act
            Categorie categorie = new Categorie(expectedId, expectedLibelle);

            // Assert
            Assert.AreEqual(expectedId, categorie.Id, "L'id doit être = a la valeur renseignée.");
            Assert.AreEqual(expectedLibelle, categorie.Libelle, "Le libelle doit être égale à la valeur renseignée.");
        }

        [TestMethod]
        public void ToString_Libelle()
        {
            // Arrange
            string expectedLibelle = "Fiction";
            Categorie categorie = new Categorie("0008", expectedLibelle);

            // Act
            string actual = categorie.ToString();

            // Assert
            Assert.AreEqual(expectedLibelle, actual, "Le libelle doit être = à la valeur renseignée.");
        }
    }
}
