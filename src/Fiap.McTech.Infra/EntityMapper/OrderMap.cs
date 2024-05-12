using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Orders;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(DataContext.Orders));

            builder.HasKey(c => c.Id);

            builder.HasMany(m => m.Items)
                .WithOne(w => w.Order)
                .HasForeignKey(c => c.OrderId)
                .IsRequired();

            builder.HasOne<Client>()
                .WithMany()
                .HasForeignKey(c => c.ClientId)
                .IsRequired();

            builder.Property(c => c.TotalAmount)
                .HasPrecision(14, 2);

            builder.Property(c => c.Status)
                .HasConversion<int>();
        }
    }
}
