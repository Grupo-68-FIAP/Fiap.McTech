using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.EntityMapper
{
    internal class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable(nameof(DataContext.Clients));
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Cpf)
                .IsRequired()
                .HasColumnType("varchar(14)")
                .HasConversion(cpf => cpf.ToString(), strCpf => new Domain.ValuesObjects.Cpf(strCpf));
            builder.Property(c => c.Email)
                .IsRequired()
                .HasColumnType("varchar(150)")
                .HasConversion(email => email.ToString(), strEmail => new Domain.ValuesObjects.Email(strEmail));
        }
    }
}
