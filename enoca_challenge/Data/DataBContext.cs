using enoca_challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace enoca_challenge.Data
{
	public class DataBContext : DbContext
	{
		public DataBContext(DbContextOptions<DataBContext> options) : base(options)
		{
			
		}

		public DbSet<Carriers> Carriers { get; set; }
		public DbSet<CarrierConfigurations> CarrierConfigurations { get; set; }
		public DbSet<Orders> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Carriers>()
				.HasKey(p => p.CarrierId);
			modelBuilder.Entity<CarrierConfigurations>()
				.HasKey(p => p.CarrierConfigurationId);
			modelBuilder.Entity<Orders>()
				.HasKey(p => p.OrderId);
		}
				
	}
}
