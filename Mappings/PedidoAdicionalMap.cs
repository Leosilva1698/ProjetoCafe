using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class PedidoAdicionalMap : IEntityTypeConfiguration<PedidoAdicionalModel>
    {
        public void Configure(EntityTypeBuilder<PedidoAdicionalModel> builder)
        {
            builder.ToTable("tb_pedido_adicionais");

            builder.HasKey(pa => new {pa.PedidoID, pa.AdicionalID})
                   .HasName("pk_pedido_adicional");

            builder.Property(pa => pa.Quantidade)
                .HasColumnType("int")
                .IsRequired();

            builder.HasData
            (
                new PedidoAdicionalModel
                {
                    PedidoID = 1,
                    AdicionalID = 1,
                    Quantidade = 1
                }
            );
        }
    }
}