using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FlightManagement.Models
{
    public class Gate
    {
        public int Id { get; set; }
        [Required]
        public string code { get; set; }
        [Required]
        public string status { get; set; }
        public List<Flight> flights { get; set; }
    }
    
}