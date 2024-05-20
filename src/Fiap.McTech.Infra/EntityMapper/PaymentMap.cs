using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fiap.McTech.Domain.Entities.Payments;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Orders;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class PaymentMap : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable(nameof(DataContext.Payments));

            builder.HasKey(c => c.Id);

            builder.HasOne<Client>()
                .WithMany()
                .HasForeignKey(c => c.ClientId);

            builder.HasOne<Order>()
                .WithMany()
                .HasForeignKey(c => c.OrderId)
                .IsRequired();

            builder.Property(c => c.Value)
                .HasPrecision(14, 2)
                .IsRequired();

            builder.Property(c => c.Discount)
                .HasPrecision(14, 2)
                .IsRequired();

            builder.Property(c => c.AdditionalFees)
                .HasPrecision(14, 2)
                .IsRequired();

            builder.Property(c => c.Method)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(c => c.Notes)
                .IsRequired();
        }
    }
}
