using Microsoft.EntityFrameworkCore;
using TicketLightAPI.Models;

namespace TicketLightAPI.Data
{
    public class TicketLightContext : DbContext
    {
        public TicketLightContext(DbContextOptions<TicketLightContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<BenefitCategory> BenefitCategories { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
    }
}
