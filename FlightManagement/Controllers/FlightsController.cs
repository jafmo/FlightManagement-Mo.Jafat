using FlightManagement.DAL;
using FlightManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FlightManagement.Controllers
{
    public class FlightsController : ApiController
    {
         private FlightRepository _flightRepo;
        public FlightsController() 
        {
            _flightRepo = new FlightRepository();
        }
        public FlightsController(List<Gate> dbList) 
        {
            _flightRepo = new FlightRepository(dbList);
        }
        [HttpGet]
        public List<Flight> getAll(int gateId)
        {
            return _flightRepo.getAll(gateId);
        }
        [HttpGet]
        public Flight getById(int gateId, int id)
        {
            return _flightRepo.getById(gateId,id);
        }
        [HttpPost]
        public Dictionary<string, object> addFlight([FromBody]Flight flight)
        {
            return _flightRepo.addFlight(flight);
        }
        [HttpPost]
        public Dictionary<string, object> updateFlight(int selectedGateId, [FromBody]Flight flight, bool pushOverlapForward)
        {
            return _flightRepo.updateFlight(selectedGateId, flight, pushOverlapForward);
        }
        [HttpPost]
        public bool DeleteFlight(int gateId, int id)
        {
            return _flightRepo.deleteFlight(gateId,id);
        }
        [HttpGet]
        public Flight createNewFlight(int gateId)
        {
            return _flightRepo.createNewFlight(gateId);
        }
    }
}
