using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoCafe.Models;

namespace ProjetoCafe.Mappings
{
    public class FuncionarioMap : IEntityTypeConfiguration<FuncionarioModel>
    {
        public void Configure(EntityTypeBuilder<FuncionarioModel> builder)
        {
            builder.ToTable("tb_funcionarios");

            builder.HasKey(f => f.FuncionarioID)
                   .HasName("pk_funcionario");

            builder.Property(f => f.Nome)
                .HasColumnType("varchar(60)")
                .IsRequired();

            builder.HasData
            (
                new FuncionarioModel
                {
                    FuncionarioID = 1, 
                    Nome = "Pedro"
                },
                new FuncionarioModel
                {
                    FuncionarioID = 2,
                    Nome = "Andre"
                }
            );
        }
    }
}