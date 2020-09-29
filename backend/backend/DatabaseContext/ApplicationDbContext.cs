using backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.DatabaseContext
{
    // IdentityDbContext contains all the user tables
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
    }
}
