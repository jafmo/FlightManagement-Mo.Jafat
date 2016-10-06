using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightManagement;
using FlightManagement.Controllers;
using FlightManagement.Models;

namespace FlightManagement.Tests.Controllers
{
    [TestClass]
    public class GatesControllerTest
    {
        private GatesController controller;

        private List<Gate> getList()
        {
            List<Gate> gatesList = new List<Gate>();
            gatesList.Add(new Gate { Id = 1, code = "G1", status = "Open", flights = new List<Flight>() });
            gatesList.Add(new Gate { Id = 2, code = "G2", status = "Open", flights = new List<Flight>() });
            return gatesList;
        }
        [TestMethod]
        public void Gate_GetAll_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new GatesController(gatesList);
            // Act
            List<Gate> result = controller.getAll();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, result.ElementAt(0).Id);
            Assert.AreEqual(2, result.ElementAt(1).Id);
        }
        [TestMethod]
        public void Gate_CreateNewGate_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new GatesController(gatesList);
            // Act
            Gate result = controller.createNewGate();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Id);
            Assert.AreEqual("G3", result.code);
        }

        [TestMethod]
        public void Gate_CreateNewGate_NoGatesTest()
        {
            // Arrange
            List<Gate> gatesList = new List<Gate>();
            controller = new GatesController(gatesList);
            // Act
            Gate result = controller.createNewGate();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(0, gatesList.Count());
        }

        [TestMethod]
        public void Gate_GetById_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new GatesController(gatesList);
            // Act
            Gate result = controller.getById(1);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("G1", result.code);
        }

        [TestMethod]
        public void Gate_addGate_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new GatesController(gatesList);
            // Act
            Dictionary<string, object> result = controller.addGate(new Gate { Id=3, code="G3", status="Open", flights = new List<Flight>()});
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue((bool)result["success"]);
            Assert.AreEqual(3, ((Gate)result["data"]).Id);
            Assert.AreEqual(3, gatesList.Count());
        }

        [TestMethod]
        public void Gate_updateGate_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new GatesController(gatesList);
            // Act
            Dictionary<string, object> result = controller.updateGate(new Gate { Id = 2, code = "G2", status = "Closed", flights = new List<Flight>() });
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue((bool)result["success"]);
            Assert.AreEqual("Closed", ((Gate)result["data"]).status);
            Assert.AreEqual("Closed", gatesList.ElementAt(1).status);
        }

        [TestMethod]
        public void Gate_deleteGate_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new GatesController(gatesList);
            // Act
            bool result = controller.deleteGate(2);
            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(1, gatesList.Count);
        }
    }
}
