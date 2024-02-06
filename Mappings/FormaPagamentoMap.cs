using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class FormaPagamentoMap : IEntityTypeConfiguration<FormaPagamentoModel>
    {
        public void Configure(EntityTypeBuilder<FormaPagamentoModel> builder)
        {
            builder.ToTable("tb_formas_pagamento");

            builder.HasKey(fp => fp.FormaPagamentoID)
                   .HasName("pk_forma_pagamento");

            builder.Property(fp => fp.Descricao)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.HasData
            (
                new FormaPagamentoModel
                {
                    FormaPagamentoID = 1,
                    Descricao = "Pix"
                },
                new FormaPagamentoModel
                {
                    FormaPagamentoID = 2,
                    Descricao = "Debito" 
                }
            );
        }
    }
}