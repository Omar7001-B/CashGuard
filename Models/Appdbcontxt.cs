using Microsoft.EntityFrameworkCore;
namespace ThreeFriends.Models
{
    public class Appdbcontxt : DbContext
    {
      //  private readonly IConfiguration _configuration ;
        public Appdbcontxt(DbContextOptions<Appdbcontxt> options) : base(options) { }
        public Appdbcontxt() { }
		//public Appdbcontxt(){ }
		public DbSet<User>Users { get; set; }


		//public Appdbcontxt():base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
				optionsBuilder.UseSqlite("Data Source=mydatabase.db");
        }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();
        }
        */
    }
}
