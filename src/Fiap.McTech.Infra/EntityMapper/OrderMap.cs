using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(DataContext.Orders));

            builder.HasKey(order => order.Id);

            builder.HasOne(order => order.Client)
                .WithMany()
                .HasForeignKey(order => order.ClientId);

            builder.Property(order => order.TotalAmount)
                .HasPrecision(14, 2);

            builder.Property(order => order.Status)
                .HasConversion<int>();

            builder.OwnsMany(o => o.Items, item =>
            {
                item.ToTable("OrderItems");

                item.HasKey(c => c.Id);

                item.HasOne(ci => ci.Order)
                    .WithMany(cc => cc.Items)
                    .HasForeignKey(ci => ci.OrderId)
                    .IsRequired();

                item.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(c => c.ProductId)
                    .IsRequired();

                item.Property(c => c.ProductName)
                    .HasMaxLength(100)
                    .IsRequired();

                item.Property(c => c.Quantity)
                    .IsRequired();

                item.Property(c => c.Price)
                    .HasPrecision(14, 2)
                    .IsRequired();
            });

            builder.Navigation(cart => cart.Items).UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}
