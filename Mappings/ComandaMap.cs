using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class ComandaMap : IEntityTypeConfiguration<ComandaModel>
    {
        public void Configure(EntityTypeBuilder<ComandaModel> builder)
        {
            builder.ToTable("tb_comandas");

            builder.HasKey(c => c.ComandaID )
                   .HasName("pk_comanda");

            builder.Property(c => c.NumeroComanda)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(c => c.Mesa)
                .HasColumnType("int");

            builder.Property(c => c.EstaAberta)
                .HasColumnType("bit")
                .IsRequired();

            builder.HasData
            (
                new ComandaModel
                {
                    ComandaID = 1,
                    NumeroComanda = 1,
                    Mesa = null,
                    EstaAberta = true
                },
                new ComandaModel
                {
                    ComandaID = 2,
                    NumeroComanda = 2,
                    Mesa = 1,
                    EstaAberta = false
                }
            );
        }
    }
}