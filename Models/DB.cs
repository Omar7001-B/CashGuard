using Microsoft.EntityFrameworkCore;

namespace ThreeFriends.Models
{
	public class DB:DbContext
	{
		public DbSet<User> Users { get; set; }

		public DB():base()
		{
			Database.EnsureCreated();
		}
		public DB(DbContextOptions<DB> options):base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=ThreeFriends;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
			base.OnConfiguring(optionsBuilder);
		}
	}
}
