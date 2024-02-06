using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class NotaFiscalMap : IEntityTypeConfiguration<NotaFiscalModel>
    {
        public void Configure(EntityTypeBuilder<NotaFiscalModel> builder)
        {
            builder.ToTable("tb_notas_fiscais");

            builder.HasKey(nf => nf.NotaFiscalID)
                   .HasName("pk_nota_fiscal");

            builder.HasIndex(nf => nf.ComandaID)
                .IsUnique();

            builder.Property(nf => nf.DataHoraCriacao)
                .HasColumnType("datetime");

            builder.Property(nf => nf.ValorTotal)
                .HasColumnType("decimal(7,2)");

            builder.Property(nf => nf.TaxaServico)
                .HasColumnType("bit");

            builder.Property(nf => nf.Desconto)
                .HasColumnType("decimal(5,2)");

            builder.Property(nf => nf.ValorFinal)
                .HasColumnType("decimal(7,2)");

            builder.HasData
            (
                new NotaFiscalModel
                {
                    NotaFiscalID = 1,
                    DataHoraCriacao = DateTime.Now,
                    ValorTotal = 14.8,
                    TaxaServico = false,
                    Desconto = 0,
                    ValorFinal = 14.8,
                    ComandaID = 2,
                    FormaPagamentoID = 2
                }
            );
        }
    }
}