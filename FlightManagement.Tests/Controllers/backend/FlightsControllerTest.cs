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
    public class FlightsControllerTest
    {
        private FlightsController controller;

        private List<Gate> getList()
        {
            List<Gate> gatesList = new List<Gate>();
            gatesList.Add(new Gate { Id = 1, code = "G1", status = "Open", flights = new List<Flight>() });
            gatesList.Add(new Gate { Id = 2, code = "G2", status = "Open", flights = new List<Flight>() });
            //add Flights to Gate 1
            addFlightstoGate(gatesList, 1, 3);
            //add Flights to Gate 2
            addFlightstoGate(gatesList, 2, 2);
            return gatesList;
        }

        private void addFlightstoGate(List<Gate> gatesList,int gateId, int flightsCount)
        {
            DateTime arrivalStartDateTime = System.DateTime.Now;
            DateTime departureStartDateTime = System.DateTime.Now.AddMinutes(29);
            for (int i = 1; i <= flightsCount; i++)
            {
                gatesList.FirstOrDefault(gate => gate.Id == gateId).flights.Add(
                    new Flight
                    {
                        Id = i,
                        gateId = gateId,
                        code = "FL" + gateId.ToString() + i.ToString(),
                        arrival = new DateTime(
                            arrivalStartDateTime.Year,
                            arrivalStartDateTime.Month,
                            arrivalStartDateTime.Day,
                            arrivalStartDateTime.Hour,
                            arrivalStartDateTime.Minute,
                            0, 0),
                        departure = new DateTime(
                            departureStartDateTime.Year,
                            departureStartDateTime.Month,
                            departureStartDateTime.Day,
                            departureStartDateTime.Hour,
                            departureStartDateTime.Minute,
                            0, 0),
                        status = "Active"
                    });
                arrivalStartDateTime.AddMinutes(30);
                departureStartDateTime.AddMinutes(30);
            }
        }

        [TestMethod]
        public void Flight_GetAll_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new FlightsController(gatesList);
            // Act
            List<Flight> result = controller.getAll(1);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(1, result.ElementAt(0).Id);
            Assert.AreEqual(2, result.ElementAt(1).Id);
        }

        [TestMethod]
        public void Flight_GetById_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new FlightsController(gatesList);
            // Act
            Flight result = controller.getById(1,1);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("FL11", result.code);
        }

        [TestMethod]
        public void Flight_addFlight_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new FlightsController(gatesList);
            // Act
            DateTime nextArrival = gatesList.FirstOrDefault(g => g.Id == 1).flights.Last().arrival.AddMinutes(30);
            DateTime nextDeparture = nextArrival.AddMinutes(29);
            Dictionary<string, object> result = controller.addFlight(new Flight { Id = 4, gateId = 1, code = "FL14", status = "Active", arrival = nextArrival, departure= nextDeparture });
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue((bool)result["success"]);
            Assert.AreEqual(4, ((Flight)result["data"]).Id);
            Assert.AreEqual(4, gatesList.ElementAt(0).flights.Count());
        }

        [TestMethod]
        public void Flight_addFlight_with_overlap_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new FlightsController(gatesList);
            // Act
            Dictionary<string, object> result = controller.addFlight(new Flight { Id = 4, gateId = 1, code = "FL14", status = "Active", arrival = DateTime.Now, departure = DateTime.Now.AddMinutes(59) });
            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse((bool)result["success"]);
            Assert.IsTrue(result["error"].ToString().IndexOf("Flight arrival time overlaps with flight") > -1);
            Assert.AreEqual(4, ((Flight)result["data"]).Id);
            Assert.AreEqual(3, gatesList.ElementAt(0).flights.Count());
        }

        [TestMethod]
        public void Flight_updateFlight_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new FlightsController(gatesList);
            // Act
            DateTime nextArrival = gatesList.FirstOrDefault(g => g.Id == 1).flights.Last().arrival.AddMinutes(30);
            DateTime nextDeparture = nextArrival.AddMinutes(29);
            Dictionary<string, object> result = controller.updateFlight(1, new Flight { Id = 1, gateId = 1, code = "FL14", status = "Cancelled", arrival = nextArrival, departure = nextDeparture}, true);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue((bool)result["success"]);
            Assert.AreEqual("Cancelled", gatesList.ElementAt(0).flights.ElementAt(0).status);
        }

        [TestMethod]
        public void Flight_updateFlight_With_Overlap_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new FlightsController(gatesList);
            // Act
            Dictionary<string, object> result = controller.updateFlight(1, new Flight { Id = 1, gateId = 1, code = "FL14", status = "Active", arrival = DateTime.Now, departure = DateTime.Now.AddMinutes(59) }, false);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse((bool)result["success"]);
            Assert.IsTrue(result["error"].ToString().IndexOf("Flight arrival time overlaps with flight") > -1);
        }

        [TestMethod]
        public void Flight_updateFlight_With_Overlap_PushForward_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new FlightsController(gatesList);
            // Act
            DateTime arrivalDateTime = DateTime.Now;
            DateTime departureDateTime = arrivalDateTime.AddMinutes(29);
            Dictionary<string, object> result = controller.updateFlight(1, new Flight { Id = 1, gateId = 1, code = "FL14", status = "Active", arrival = arrivalDateTime, departure = departureDateTime }, true);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue((bool)result["success"]);
            Assert.AreEqual(arrivalDateTime, ((Flight)result["data"]).arrival);
        }

        [TestMethod]
        public void Flight_deleteFlight_Test()
        {
            // Arrange
            List<Gate> gatesList = getList();
            controller = new FlightsController(gatesList);
            // Act
            bool result = controller.DeleteFlight(1,2);
            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(2, gatesList.ElementAt(0).flights.Count());
        }
    }
}
