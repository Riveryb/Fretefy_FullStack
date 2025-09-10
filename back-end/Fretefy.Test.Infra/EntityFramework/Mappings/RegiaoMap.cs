using Fretefy.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fretefy.Test.Infra.EntityFramework.Mappings
{
    public class RegiaoMap : IEntityTypeConfiguration<Regiao>
    {
        public void Configure(EntityTypeBuilder<Regiao> builder)
        {
            builder.ToTable("Regiao");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Nome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(r => r.Ativo)
                   .IsRequired();

            builder.HasIndex(r => r.Nome).IsUnique(); // nome único
        }
    }
}
