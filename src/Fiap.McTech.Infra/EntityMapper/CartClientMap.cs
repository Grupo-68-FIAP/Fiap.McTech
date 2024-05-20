using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fiap.McTech.Domain.Entities.Cart;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class CartClientMap : IEntityTypeConfiguration<CartClient>
    {
        public void Configure(EntityTypeBuilder<CartClient> builder)
        {
            builder.ToTable(nameof(DataContext.CartClients));

            builder.HasKey(cart => cart.Id);

            builder.HasMany(cart => cart.Items)
                .WithOne(cart => cart.CartClient)
                .HasForeignKey(cart => cart.CartClientId);

            builder.HasOne(cart => cart.Client)
                .WithMany()
                .HasForeignKey(cart => cart.ClientId);

            builder.Property(cart => cart.AllValue)
                .HasPrecision(14,2);
        }
    }
}
