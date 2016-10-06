using FlightManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlightManagement.DAL
{
    public interface IFlightRepository
    {
        List<Flight> getAll(int gateId);
        Flight getById(int gateId,int id);
        Dictionary<string, object> addFlight(Flight flight);
        Dictionary<string, object> updateFlight(int gateId, Flight flight, bool pushOverlapForward);
        bool deleteFlight(int gateId,int flightId);
    }
}
