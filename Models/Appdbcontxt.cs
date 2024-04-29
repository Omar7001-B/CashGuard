using Microsoft.EntityFrameworkCore;
namespace ThreeFriends.Models
{
    public class Appdbcontxt : DbContext
    {
        public Appdbcontxt(DbContextOptions<Appdbcontxt> options) : base(options) { }
        public Appdbcontxt() { }
		public DbSet<User> Users { get; set; }
		public DbSet<HistoryItem> History { get; set; }
		public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { optionsBuilder.UseSqlite("Data Source=DataBase\\CoinGuard.db"); }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.Id); // Specify the primary key

            // Define the relationship between Transaction and Category
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
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
// optionsBuilder.UseSqlServer("Data Source=DESKTOP-1ULGF16\\SQLEXPRESS;Initial Catalog = CashGaurd ;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
*/
