using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class CartClientMap : IEntityTypeConfiguration<CartClient>
    {
        public void Configure(EntityTypeBuilder<CartClient> builder)
        {
            builder.ToTable(nameof(DataContext.CartClients));

            builder.HasKey(cart => cart.Id);

            builder.HasOne(cart => cart.Client)
                .WithMany()
                .HasForeignKey(cart => cart.ClientId);

            builder.Property(cart => cart.AllValue)
                .HasPrecision(14, 2);

            builder.HasData(new CartClient(Guid.NewGuid(), 0));

            builder.OwnsMany(cart => cart.Items, items =>
            {
                items.ToTable("CartItems");

                items.HasKey(item => item.Id);

                items.WithOwner(item => item.CartClient)
                    .HasForeignKey(item => item.CartClientId);

                items.HasOne(item => item.Product)
                    .WithMany()
                    .HasForeignKey(item => item.ProductId)
                    .IsRequired();

                items.Property(item => item.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                items.Property(item => item.Quantity)
                    .IsRequired();

                items.Property(item => item.Value)
                    .HasPrecision(14, 2)
                    .IsRequired();
            });

            builder.Navigation(cart => cart.Items).UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}
