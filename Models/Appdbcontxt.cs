using Microsoft.EntityFrameworkCore;
namespace ThreeFriends.Models
{
    public class Appdbcontxt : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<User>Users { get; set; }

        public Appdbcontxt(IConfiguration configuration):base()
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string constring = _configuration.GetConnectionString("DefaultConnection"); 
            optionsBuilder.UseSqlServer(constring);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();
        }
    }
}
