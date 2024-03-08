using Microsoft.EntityFrameworkCore;

namespace ThreeFriends.Models
{
	public class DB:DbContext
	{
		public DbSet<User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=ThreeFriends;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
		}
	}
}
