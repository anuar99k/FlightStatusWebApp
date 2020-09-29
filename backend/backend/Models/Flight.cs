using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }

        [Required]
        public string DepartureCity { get; set; }

        [Required]
        public string ArrivalCity { get; set; }
        public TimeSpan? DelayTime { get; set; }
    }
}
