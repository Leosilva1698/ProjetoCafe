using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<ClienteModel>
    {
        public void Configure(EntityTypeBuilder<ClienteModel> builder)
        {
            builder.ToTable("tb_clientes");

            builder.HasKey(c => c.ClienteID)
                .HasName("pk_cliente");

            builder.Property(c => c.Nome)
                .HasColumnType("varchar(70)")
                .IsRequired();

            builder.Property(c => c.Telefone)
                .HasColumnType("char(11)");

            builder.Property(c => c.Email)
                .HasColumnType("varchar(50)");    

            builder.Property(c => c.CPF)
                .HasColumnType("char(11)")
                .IsRequired();

            builder.HasData
            (
                new ClienteModel
                {
                    ClienteID = 1,
                    Nome = "Leonardo",
                    Telefone = "41912332112",
                    Email = "leonardo@example.com",
                    CPF = "11122233345"
                },
                new ClienteModel
                {
                    ClienteID = 2,
                    Nome = "Guilherme",
                    Telefone = "41998765432",
                    Email = "guilherme@example.com",
                    CPF = "33322211154"
                }
            );
        }
    }
}