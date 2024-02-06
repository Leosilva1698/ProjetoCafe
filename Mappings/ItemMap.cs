using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class ItemMap : IEntityTypeConfiguration<ItemModel>
    {
        public void Configure(EntityTypeBuilder<ItemModel> builder)
        {
            builder.ToTable("tb_itens");

            builder.HasKey(i => i.ItemID)
                   .HasName("pk_item");

            builder.Property(i => i.Nome)
                .HasColumnType("varchar(30)")
                .IsRequired();

            builder.Property(i => i.Descricao)
                .HasColumnType("varchar(45)");

            builder.Property(i => i.Tamanho)
                .HasColumnType("char(1)");

            builder.Property(i => i.Valor)
                .HasColumnType("decimal(5,2)");

            builder.HasData
            (
                new ItemModel
                {
                    ItemID = 1,
                    Nome = "Cappuccino",
                    Descricao = null,
                    Tamanho = "M",
                    Valor = 10.9
                },
                new ItemModel
                {
                    ItemID = 2,
                    Nome = "Cafe Expresso",
                    Descricao = null,
                    Tamanho = "P",
                    Valor = 7.5
                }
            );
        }
    }
}