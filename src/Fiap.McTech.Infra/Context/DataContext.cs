﻿using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Entities.Payments;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Infra.EntityMapper;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.Context
{
    public class DataContext : DbContext
    {
        public DbSet<CartClient>? CartClients { get; set; }
        public DbSet<Client>? Clients { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Payment>? Payments { get; set; }

        public DataContext() : base() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;
            // configure database to run migrations
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
                throw new DatabaseException("Environment variable [CONNECTION_STRING] is null.");
            optionsBuilder.UseSqlServer(connectionString);
            Console.WriteLine("Connected: {0}", connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartClientMap());
            modelBuilder.ApplyConfiguration(new ClientMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new PaymentMap());
        }
    }
}
