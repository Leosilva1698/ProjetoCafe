using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class AdicionalMap : IEntityTypeConfiguration<AdicionalModel>
    {
        public void Configure(EntityTypeBuilder<AdicionalModel> builder)
        {
            builder.ToTable("tb_adicionais");

            builder.HasKey(a => a.AdicionalID)
                   .HasName("pk_adicional");

            builder.Property(a => a.Descricao)
                .HasColumnType("varchar(30)")
                .IsRequired();

            builder.Property(a => a.Valor)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.HasData
            (
                new AdicionalModel
                {
                    AdicionalID = 1,
                    Descricao = "Chantilly",
                    Valor = 3.90
                },
                new AdicionalModel
                {
                    AdicionalID = 2,
                    Descricao = "Ovo",
                    Valor = 1
                }
            );
        }
    }
}