using Fiap.McTech.Domain.Entities.Orders;
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

            builder.HasMany(order => order.Items)
                .WithOne(order => order.Order)
                .HasForeignKey(order => order.OrderId);

            builder.HasOne(order => order.Client)
                .WithMany()
                .HasForeignKey(order => order.ClientId);

            builder.Property(order => order.TotalAmount)
                .HasPrecision(14, 2);

            builder.Property(order => order.Status)
                .HasConversion<int>();
        }
    }
}
