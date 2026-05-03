using Microsoft.EntityFrameworkCore;
using graduate_proj.Models;

namespace graduate_proj.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Ride> Rides { get; set; }

        // تأكدي إن مفيش دالة هنا اسمها OnConfiguring فيها UseSqlServer
    }
}