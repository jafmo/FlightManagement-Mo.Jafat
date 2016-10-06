using FlightManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace FlightManagement.DAL
{
    public class BaseRepository : IRequiresSessionState
    {
        public List<Gate> _db { get; set; }
        public BaseRepository()
        {
            if (HttpContext.Current.Application["db"] != null)
            {
                _db = (List<Gate>)HttpContext.Current.Application["db"];
            }
            
            else
            {
                _db = init();
                HttpContext.Current.Application.Add("db", _db);
            }
        }
        public BaseRepository(List<Gate> db)
        {
            _db = db;
        }

        public List<Gate> init()
        {
            var _db = new List<Gate>();
            //add gates
            _db.Add(new Gate { Id = 1, code = "G1", status = "Open", flights = new List<Flight>() });
            _db.Add(new Gate { Id = 2, code = "G2", status = "Open", flights = new List<Flight>() });
            //add flights to gate 1
            addFlightstoGate(_db,1,10);
            //add flights to gate 2
            addFlightstoGate(_db, 2, 10);
            return _db;
        }

        private void addFlightstoGate(List<Gate> gatesList, int gateId, int flightsCount)
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
                            0,0),
                        departure = new DateTime(
                            departureStartDateTime.Year,
                            departureStartDateTime.Month,
                            departureStartDateTime.Day,
                            departureStartDateTime.Hour,
                            departureStartDateTime.Minute,
                            0, 0),
                        status = "Active"
                    });
                arrivalStartDateTime = arrivalStartDateTime.AddMinutes(30);
                departureStartDateTime = departureStartDateTime.AddMinutes(30);
            }
        }
    }
}