using ChatRealtimeDemo.Models;
using ChatRealtimeDemo.Models.Donate;
using Microsoft.EntityFrameworkCore;

namespace ChatRealtimeDemo.Data
{
    public class ChatRealtimeDbContext : DbContext
    {
        public ChatRealtimeDbContext(DbContextOptions<ChatRealtimeDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<ChatMessages> Messages { get; set; }
        public DbSet<Donation> Donations { get; set; }
    }
}
