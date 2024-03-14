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
    public class ServiceTests
    {
        [TestMethod]
        public void Service_Test()
        {
            // Arrange
            Service service = new Service();
            int expectedId = 1;
            string expectedNom = "Informatique";
            string expectedDescription = "Service chargé de la gestion des systèmes informatiques.";

            // Act
            service.Id = expectedId;
            service.Nom = expectedNom;
            service.Description = expectedDescription;

            // Assert
            Assert.AreEqual(expectedId, service.Id);
            Assert.AreEqual(expectedNom, service.Nom);
            Assert.AreEqual(expectedDescription, service.Description);
        }
    }
}
