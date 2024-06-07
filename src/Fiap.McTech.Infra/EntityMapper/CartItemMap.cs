using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class CartItemMap : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable(nameof(DataContext.CartItems));

            builder.HasKey(c => c.Id);

            builder.HasOne(ci => ci.CartClient)
                .WithMany(cc => cc.Items)
                .HasForeignKey(ci => ci.CartClientId)
                .IsRequired();

            builder.HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .IsRequired();

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Quantity)
                .IsRequired();

            builder.Property(c => c.Value)
                .HasPrecision(14, 2)
                .IsRequired();
        }
    }
}
