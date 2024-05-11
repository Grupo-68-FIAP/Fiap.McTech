using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Fiap.McTech.Domain.Entities.Products;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(DataContext.Products));
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(c => c.Value)
                .HasPrecision(14,2)
                .IsRequired();
            builder.Property(c => c.Description)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(c => c.Image).IsRequired();
            builder.Property(c => c.Category)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}
