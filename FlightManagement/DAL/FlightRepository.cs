using FlightManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightManagement.DAL
{
    public class FlightRepository:BaseRepository,IFlightRepository
    {
        public FlightRepository(): base()
        {
        }
        public FlightRepository(List<Gate> db): base(db)
        {
            _db = db;
        }
        public List<Flight> getAll(int gateId)
        {
            DateTime fromDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
            DateTime toDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,23,59,0);
            List<Flight> flights = _db.FirstOrDefault(g => g.Id == gateId).flights;
            List<Flight> todaysFlights = new List<Flight>();
            if (flights != null)
            {
                todaysFlights = flights
                    .Where(f => (f.arrival >= fromDate & f.arrival <= toDate) || (f.departure >= fromDate & f.departure <= toDate)).ToList();
            }
            return todaysFlights;
        }
        public Flight getById(int gateId, int id)
        {
            return _db.FirstOrDefault(g => g.Id == gateId).flights.FirstOrDefault(f=>f.Id == id);
        }
        public Dictionary<string, object> addFlight(Flight flight)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            int overlapIndex = 0;
            string errorStr = "";
            try
            {
                //validate 
                if (!isValidFlight(flight, out errorStr))
                {
                    result.Add("success", false);
                    result.Add("data", flight);
                    result.Add("error", errorStr);
                    return result;
                }
                if (!flightOverlap(flight, out overlapIndex))
                {
                    // Initialise flights list for new gates
                    if (_db.FirstOrDefault(g => g.Id == flight.gateId).flights == null)
                    {
                        _db.FirstOrDefault(g => g.Id == flight.gateId).flights = new List<Flight>();
                    }
                    _db.FirstOrDefault(g => g.Id == flight.gateId).flights.Add(flight);
                    result.Add("success", true);
                    result.Add("data", flight);
                    result.Add("error", "");
                }
                else //overlaps
                {
                    Flight overlapFlight = _db.FirstOrDefault(g => g.Id == flight.gateId)
                                            .flights.ElementAt(overlapIndex);
                    DateTime nextAvailableTime = _db.FirstOrDefault(g => g.Id == flight.gateId)
                                            .flights.Max(f => f.arrival).AddMinutes(30);
                    result.Add("success", false);
                    result.Add("data", flight);
                    result.Add("error", "Flight arrival time overlaps with flight " +
                        overlapFlight.code +
                        ". Next available arrival time is " + nextAvailableTime.ToString("dd/MM/yyyy HH:mm tt"));
                }
            }
            catch(Exception ex)
            {
                result.Add("success", false);
                result.Add("data", flight);
                result.Add("error", ex.InnerException.Message);
            }
            return result;
        }
        public Dictionary<string, object> updateFlight(int selectedGateId, Flight flight, bool pushOverlapForward)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            int overlapIndex = 0;
            string errorStr = "";
            try
            {
                //validate 
                if (!isValidFlight(flight, out errorStr))
                {
                    result.Add("success", false);
                    result.Add("data", flight);
                    result.Add("error", errorStr);
                    return result;
                }
                if (!flightOverlap(flight, out overlapIndex))
                {
                    Flight originalFlight = _db.FirstOrDefault(g => g.Id == flight.gateId)
                                            .flights.FirstOrDefault(f => f.Id == flight.Id);
                    if (originalFlight != null)
                    {
                        originalFlight.code = flight.code;
                        originalFlight.status = flight.status;
                        originalFlight.arrival = flight.arrival;
                        originalFlight.departure = flight.departure;

                        // move to different gate if assigned to a new gateId
                        if (originalFlight.gateId != selectedGateId)
                        {
                            // Initialise flights list for new gates
                            if (_db.FirstOrDefault(g => g.Id == selectedGateId).flights == null)
                            {
                                _db.FirstOrDefault(g => g.Id == selectedGateId).flights = new List<Flight>();
                            }

                            _db.FirstOrDefault(g => g.Id == originalFlight.gateId).flights.Remove(originalFlight);
                            originalFlight.gateId = selectedGateId;
                            int nextId = (_db.FirstOrDefault(g => g.Id == selectedGateId).flights.Count == 0) ? 1 : _db.FirstOrDefault(g => g.Id == selectedGateId).flights.Last().Id + 1;
                            originalFlight.Id = nextId;
                            _db.FirstOrDefault(g => g.Id == selectedGateId).flights.Add(originalFlight);
                        }
                    }
                    result.Add("success", true);
                    result.Add("data", flight);
                    result.Add("error", "");
                }
                else //overlaps
                {
                    if (pushOverlapForward == true)
                    {
                        /*** Push flights forward ***/
                        bool pushed = pushFlightsForward(flight);
                        if (pushed == true)
                        {
                            result.Add("success", true);
                            result.Add("data", flight);
                            result.Add("error", "");
                        }
                    }
                    else
                    {
                        result.Add("success", false);
                        result.Add("data", flight);
                        result.Add("error", "Flight arrival time overlaps with flight " +
                            _db.FirstOrDefault(g => g.Id == flight.gateId)
                                                .flights.ElementAt(overlapIndex).code +
                            ". To resolve the overlap issue, You can try to select a different Gate or select to push overlap flights forward.");
                    }
                }
            }
            catch(Exception ex)
            {
                result.Add("success", false);
                result.Add("data", flight);
                result.Add("error", ex.InnerException.Message);
            }
            return result;
        }
        private bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }
        private bool isValidFlight(Flight flight, out string errorStr)
        {
            errorStr = "";
            bool result = true;
                if (flight.gateId == 0)
                {
                    errorStr += "Flight Gate is required. ";
                    result = false;
                }
                if (flight.code == null || flight.code.Trim() == string.Empty)
                {
                    errorStr+= "Flight Code is required. ";
                    result = false;
                }
                if (flight.status == null || flight.status.Trim() == string.Empty)
                {
                    errorStr += "Flight Status is required. ";
                    result = false;
                }
                if (flight.arrival == null)
                {
                    errorStr += "Flight Arrival Date is required. ";
                    result = false;
                }
                else
                {
                    if (!IsDateTime(flight.arrival.ToString()))
                    {
                        errorStr += "Flight Arrival Date is not valid. ";
                        result = false;
                    }
                }
                if (flight.departure == null)
                {
                    errorStr += "Flight Departure Date is required. ";
                    result = false;
                }
                else
                {
                    if (!IsDateTime(flight.departure.ToString()))
                    {
                        errorStr += "Flight departure Date is not valid. ";
                        result = false;
                    }
                }
                return result;
        }
        private bool pushFlightsForward(Flight updateFlight)
        {
            bool result = true;
            List<Flight> flightsList = _db.FirstOrDefault(g => g.Id == updateFlight.gateId).flights;
            // update flight 
            Flight originalFlight = flightsList.FirstOrDefault(f => f.Id == updateFlight.Id);
            originalFlight.arrival = updateFlight.arrival;
            originalFlight.departure = originalFlight.arrival.AddMinutes(29);
            int updatedFlightIndex = flightsList.IndexOf(originalFlight);
            // 0 to update index
            for (int i = updatedFlightIndex-1; i >= 0; i--)
            {
                flightsList[i].arrival = flightsList[i+1].arrival.AddMinutes(-30);
                flightsList[i].departure = flightsList[i].arrival.AddMinutes(29);
            }
            //update index to end
            for (int i = updatedFlightIndex + 1; i < flightsList.Count; i++)
            {
                flightsList[i].arrival = flightsList[i - 1].arrival.AddMinutes(30);
                flightsList[i].departure = flightsList[i].arrival.AddMinutes(29);
            }
            return result;
        }
        private bool flightOverlap(Flight flight, out int overlapIndex)
        {
            bool result = false;
            int index = 0;
            if (_db.FirstOrDefault(g => g.Id == flight.gateId).flights != null)
            {
                List<Flight> otherFlightsList = _db.FirstOrDefault(g => g.Id == flight.gateId)
                                        .flights.Where(f => f.Id != flight.Id).ToList();
                foreach (var f in otherFlightsList)
                {
                    TimeSpan span = flight.arrival.Subtract(f.arrival);
                    if (Math.Abs(span.Hours) == 0 && Math.Abs(span.Minutes) < 30)
                    {
                        result = true;
                        break;
                    }
                    index += 1;
                }
            }
            overlapIndex = index;
            return result;
        }
        public bool deleteFlight(int gateId,int flightId)
        {
            Flight flightToDelete = _db.FirstOrDefault(g => g.Id == gateId).flights.FirstOrDefault(f => f.Id == flightId);
            _db.FirstOrDefault(g => g.Id == gateId).flights.Remove(flightToDelete);
            return true;
        }
        public Flight createNewFlight(int gateId)
        {
            int nextId = (_db.FirstOrDefault(g => g.Id == gateId).flights == null) ? 1 : _db.FirstOrDefault(g => g.Id == gateId).flights.Last().Id + 1;
            // get last flight arrival. If none then get date/time now and take out seconds for overlap minutes accuracy 
            DateTime lastFlightArrival = (_db.FirstOrDefault(g => g.Id == gateId).flights == null)?
                DateTime.Now.AddSeconds(-DateTime.Now.Second): 
                _db.FirstOrDefault(g => g.Id == gateId).flights.Last().arrival;
            Flight newFlight = new Flight { 
                Id = nextId, code = "FL" + gateId.ToString() + nextId.ToString(), 
                gateId = gateId,
                status = "Active",
                arrival = lastFlightArrival.AddMinutes(30),
                departure = lastFlightArrival.AddMinutes(59)
            };
            return newFlight;
        }
    }
}