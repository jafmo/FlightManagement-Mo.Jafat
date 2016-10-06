using FlightManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlightManagement.DAL
{
    public interface IGateRepository
    {
        List<Gate> getAll();
        Gate getById(int id);
        Dictionary<string, object> addGate(Gate gate);
        Dictionary<string, object> updateGate(Gate gate);
        bool deleteGate(int gateId);
        Gate createNewGate();
    }
}
