using FlightManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightManagement.DAL
{
    public class GateRepository:BaseRepository,IGateRepository
    {
        public GateRepository():base()
        {
        }
        public GateRepository(List<Gate> db):base(db)
        {
            _db = db;
        }
        public List<Gate> getAll()
        {
            return _db;
        }
        public Gate getById(int id)
        {
            return _db.FirstOrDefault(g => g.Id == id);
        }
        public Dictionary<string, object> addGate(Gate gate)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string errorStr = "";
            try
            {
                if (!isValidGate(gate, out errorStr))
                {
                    result.Add("success", false);
                    result.Add("data", gate);
                    result.Add("error", errorStr);
                    return result;
                }
                _db.Add(gate);
                result.Add("success", true);
                result.Add("data", gate);
                result.Add("error", "");
            }
            catch(Exception ex)
            {
                result.Add("success", false);
                result.Add("data", gate);
                result.Add("error", ex.InnerException.Message);
            }
            return result;
        }
        public Dictionary<string,object> updateGate(Gate gate)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            string errorStr = "";
            try
            {
                if (!isValidGate(gate, out errorStr))
                {
                    result.Add("success", false);
                    result.Add("data", gate);
                    result.Add("error", errorStr);
                    return result;
                }

                Gate originalGate = _db.FirstOrDefault(g => g.Id == gate.Id);
                if (originalGate != null)
                {
                    originalGate.code = gate.code;
                    originalGate.status = gate.status;
                }
                result.Add("success", true);
                result.Add("data", gate);
                result.Add("error", "");
            }
            catch(Exception ex)
            {
                result.Add("success", false);
                result.Add("data", gate);
                result.Add("error", ex.InnerException.Message);
            }
            return result;
        }

        private bool isValidGate(Gate gate, out string errorStr)
        {
            errorStr = "";
            bool result = true;
            if (gate.code == null || gate.code.Trim() == string.Empty)
            {
                errorStr += "Gate Code is required. ";
                result = false;
            }
            if (gate.status == null || gate.status.Trim() == string.Empty)
            {
                errorStr += "Gate status is required. ";
                result = false;
            }
            return result;
        }
        public bool deleteGate(int gateId)
        {
            _db.Remove(_db.FirstOrDefault(g => g.Id == gateId));
            return true;
        }
        public Gate createNewGate()
        {
            int nextId = (_db.Count == 0) ? 1 : _db.Last().Id + 1;
            Gate newGate = new Gate { Id = nextId, code = "G" + nextId.ToString(), status = "Open", flights = new List<Flight>() };
            return newGate;
        }
    }
}