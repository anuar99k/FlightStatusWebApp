using backend.DatabaseContext;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        public FlightsController(ApplicationDbContext dbContext) => _dbContext = dbContext;

        // GET: api/flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            return await _dbContext.Flights.ToListAsync();
        }

        // GET: api/flights/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            Flight flight = await _dbContext.Flights.FindAsync(id);

            if (flight == null)
                return NotFound();

            return flight;
        }

        // PUT: api/flights/{id}
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> PutFlight(int id, Flight flight)
        {
            if (id != flight.FlightId)
                return BadRequest();

            _dbContext.Entry(flight).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
                    BadRequest();
                else
                    throw;
            }
            return NoContent();
        }

        // POST: api/flights
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
            _dbContext.Flights.Add(flight);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFlight), new { id = flight.FlightId }, flight);
        }

        // DELETE: api/flights/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Flight>> DeleteFlight(int id)
        {
            Flight flight = await _dbContext.Flights.FindAsync(id);

            if (flight == null)
                return NotFound();

            _dbContext.Flights.Remove(flight);

            await _dbContext.SaveChangesAsync();

            return flight;
        }

        private bool FlightExists(int id)
        {
            return _dbContext.Flights.Any(f => f.FlightId == id);
        }
    }
}
