using Fretefy.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Fretefy.Test.Infra.EntityFramework.Mappings
{
    public class CidadeMap : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder.ToTable("Cidade");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(p => p.UF)
                   .HasMaxLength(2)
                   .IsRequired();

            // Evita duplicar a mesma cidade/UF
            builder.HasIndex(c => new { c.Nome, c.UF }).IsUnique();

            // (Opcional) SEED — se quiser manter, use GUIDs fixos (não use Guid.NewGuid() no construtor).
            // Obs.: Como seu construtor gera Id, o HasData precisa de Ids explícitos aqui.
            // Descomente se quiser semear. Ajuste os GUIDs conforme necessário.

            /*
            builder.HasData(
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Nome = "Rio Branco",     UF = "AC" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaab"), Nome = "Maceió",         UF = "AL" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaac"), Nome = "Macapá",         UF = "AP" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaad"), Nome = "Manaus",         UF = "AM" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaae"), Nome = "Salvador",       UF = "BA" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaf"), Nome = "Fortaleza",      UF = "CE" }, // <- sem espaço
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab0"), Nome = "Brasília",       UF = "DF" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab1"), Nome = "Vitória",        UF = "ES" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab2"), Nome = "Goiânia",        UF = "GO" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab3"), Nome = "São Luís",       UF = "MA" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab4"), Nome = "Cuiabá",         UF = "MT" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab5"), Nome = "Campo Grande",   UF = "MS" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab6"), Nome = "Belo Horizonte", UF = "MG" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab7"), Nome = "Belém",          UF = "PA" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab8"), Nome = "João Pessoa",    UF = "PB" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab9"), Nome = "Curitiba",       UF = "PR" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac0"), Nome = "Recife",         UF = "PE" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac1"), Nome = "Teresina",       UF = "PI" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac2"), Nome = "Rio de Janeiro", UF = "RJ" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac3"), Nome = "Natal",          UF = "RN" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac4"), Nome = "Porto Alegre",   UF = "RS" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac5"), Nome = "Porto Velho",    UF = "RO" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac6"), Nome = "Boa Vista",      UF = "RR" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac7"), Nome = "Florianópolis",  UF = "SC" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac8"), Nome = "São Paulo",      UF = "SP" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaac9"), Nome = "Aracaju",        UF = "SE" },
                new Cidade { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaad0"), Nome = "Palmas",         UF = "TO" }
            );
            */
        }
    }
}
