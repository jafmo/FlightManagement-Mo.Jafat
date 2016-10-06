using FlightManagement.DAL;
using FlightManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FlightManagement.Controllers
{
    public class GatesController : ApiController
    {
        private GateRepository _gateRepo;
        
        public GatesController()
        {
            _gateRepo = new GateRepository();
        }
        public GatesController(List<Gate> dbList)
        {
            _gateRepo = new GateRepository(dbList);
        }
        [HttpGet]
        public List<Gate> getAll()
        {
            return _gateRepo.getAll();
        }
        [HttpGet]
        public Gate getById(int id)
        {
            return _gateRepo.getById(id);
        }
        [HttpGet]
        public Gate createNewGate()
        {
            return _gateRepo.createNewGate();
        }
        [HttpPost]
        public Dictionary<string, object> addGate([FromBody]Gate gate)
        {
            return _gateRepo.addGate(gate);
        }
        [HttpPost]
        public Dictionary<string, object> updateGate([FromBody]Gate gate)
        {
            return _gateRepo.updateGate(gate);
        }
        [HttpPost]
        public bool deleteGate(int id)
        {
            return _gateRepo.deleteGate(id);
        }
    }
}
