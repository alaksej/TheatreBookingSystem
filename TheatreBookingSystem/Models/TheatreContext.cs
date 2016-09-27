using System.Data.Entity;

namespace TheatreBookingSystem.Models
{
	public class TheatreContext : DbContext
	{

		public virtual DbSet<Author> Authors { get; set; }
		public virtual DbSet<Genre> Genres { get; set; }
		public virtual DbSet<Play> Plays { get; set; }
		public virtual DbSet<PlayDate> Dates { get; set; }
		public virtual DbSet<Login> Logins { get; set; }
		public virtual DbSet<Order> Orders { get; set; }

	}
}