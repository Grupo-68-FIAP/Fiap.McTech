using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Entities.Products;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class OrderItemMap : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable(nameof(DataContext.OrderItems));

            builder.HasKey(c => c.Id);

            builder.HasOne(ci => ci.Order)
                .WithMany(cc => cc.Items)
                .HasForeignKey(ci => ci.OrderId)
                .IsRequired();

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .IsRequired();

            builder.Property(c => c.ProductName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Quantity)
                .IsRequired();

            builder.Property(c => c.Price)
                .HasPrecision(14, 2)
                .IsRequired();
        }
    }
}
