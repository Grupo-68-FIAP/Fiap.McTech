using Microsoft.EntityFrameworkCore;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Catalogs;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Entities.Payments;
using Fiap.McTech.Domain.Entities.Products;

namespace Fiap.McTech.Infra.Context
{
	public class DataContext : DbContext
	{

		public DataContext(DbContextOptions<DataContext> options)
		   : base(options)
		{
		}

		public DbSet<CartClient> CartClients { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Catalog> Catalog { get; set; }
		public DbSet<Client> Clients { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
	}
}