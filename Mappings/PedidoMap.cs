using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class PedidoMap : IEntityTypeConfiguration<PedidoModel>
    {
        public void Configure(EntityTypeBuilder<PedidoModel> builder)
        {
            builder.ToTable("tb_pedidos");

            builder.HasKey(p => p.PedidoID)
                   .HasName("pk_pedido") ;

            builder.Property(p => p.HoraFeito)
                .HasColumnType("time(0)")
                .IsRequired();

            builder.Property(p => p.ValorTotal)
                .HasColumnType("decimal(5,2)");

            builder.HasData
            (
                new PedidoModel
                {
                    PedidoID = 1,
                    HoraFeito = new TimeOnly(13, 37, 20),
                    FuncionarioID = 1,
                    ComandaID = 2,
                    ValorTotal = 14.8
                }
            );
        }
    }
}