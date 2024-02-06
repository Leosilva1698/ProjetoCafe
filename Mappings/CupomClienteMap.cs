using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class CupomClienteMap : IEntityTypeConfiguration<CupomClienteModel>
    {
        public void Configure(EntityTypeBuilder<CupomClienteModel> builder)
        {
            builder.ToTable("tb_cupons_cliente");

            builder.HasKey(cc => new {cc.ClienteID, cc.NotaFiscalID})
                   .HasName("pk_cupom_cliente");

            builder.HasIndex(cc => cc.NotaFiscalID)
                .IsUnique();

            builder.Property(cc => cc.DataGeracao)
                .HasColumnType("date");

            builder.Property(cc => cc.DataValidade)
                .HasColumnType("date");

            builder.Property(cc => cc.Valor)
                .HasColumnType("decimal(5,2)");

            builder.Property(cc => cc.Valido)
                .HasColumnType("bit");

            builder.HasData(
                new CupomClienteModel
                {
                    ClienteID = 1,
                    NotaFiscalID = 1,
                    DataGeracao = new DateTime(2024, 02, 04),
                    DataValidade = new DateTime(2024, 02, 08),
                    Valor = 10,
                    Valido = true
                }
            );
            
        }
    }
}