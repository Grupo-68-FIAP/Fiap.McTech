using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Clients;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class CartClientMap : IEntityTypeConfiguration<CartClient>
    {
        public void Configure(EntityTypeBuilder<CartClient> builder)
        {
            builder.ToTable(nameof(DataContext.CartClients));

            builder.HasKey(c => c.Id);

            builder.HasMany(m => m.Items)
                .WithOne(w => w.CartClient)
                .HasForeignKey(c => c.CartClientId)
                .IsRequired();

            builder.HasOne<Client>()
                .WithMany()
                .HasForeignKey(c => c.ClientId)
                .IsRequired();

            builder.Property(c => c.AllValue)
                .HasPrecision(14,2);
        }
    }
}
