using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class PedidoItemMap : IEntityTypeConfiguration<PedidoItemModel>
    {
        public void Configure(EntityTypeBuilder<PedidoItemModel> builder)
        {
            builder.ToTable("tb_pedido_itens");

            builder.HasKey(pi => new {pi.PedidoID, pi.ItemID})
                   .HasName("pk_pedido_item");

            builder.Property(pi => pi.Observacao)
                .HasColumnType("varchar(70)");

            builder.Property(pi => pi.Quantidade)
                .HasColumnType("int")
                .IsRequired();

            builder.HasData
            (
                new PedidoItemModel
                {
                    PedidoID = 1,
                    ItemID = 1,
                    Observacao = null,
                    Quantidade = 1
                }
            );
        }
    }
}