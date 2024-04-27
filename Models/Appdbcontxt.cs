using Microsoft.EntityFrameworkCore;
namespace ThreeFriends.Models
{
    public class Appdbcontxt : DbContext
    {
        public Appdbcontxt(DbContextOptions<Appdbcontxt> options) : base(options) { }
        public Appdbcontxt() { }
		public DbSet<User>Users { get; set; }
		public DbSet<HistoryItem>History { get; set; }
		public DbSet<Category>Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // optionsBuilder.UseSqlServer("Data Source=DESKTOP-1ULGF16\\SQLEXPRESS;Initial Catalog = CashGaurd ;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

	         	optionsBuilder.UseSqlite("Data Source=DataBase\\CoinGuard.db");

        }

    }
}

/*
// Archive

//public Appdbcontxt():base() { }
//public Appdbcontxt(){ }
//  private readonly IConfiguration _configuration ;
/*
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
	modelBuilder.Entity<User>()
		.Property(u => u.Password)
		.IsRequired();
}
*/
