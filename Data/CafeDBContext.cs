using Microsoft.EntityFrameworkCore;
using ProjetoCafe.Mappings;
using ProjetoCafe.Models;

namespace ProjetoCafe.Data
{
    public class CafeDBContext : DbContext
    {
        public DbSet<ClienteModel> Cliente { get; set; } 
        public DbSet<NotaFiscalModel> NotaFiscal { get; set; }
        public DbSet<CupomClienteModel> CupomCliente { get; set; }
        public DbSet<FormaPagamentoModel> FormaPagamento { get; set; }
        public DbSet<ComandaModel> Comanda { get; set; }
        public DbSet<FuncionarioModel> Funcionario { get; set; }
        public DbSet<ItemModel> Item { get; set; }
        public DbSet<PedidoModel> Pedido { get; set; }
        public DbSet<AdicionalModel> Adicional { get; set; }
        public DbSet<PedidoItemModel> PedidoItem { get; set; }
        public DbSet<PedidoAdicionalModel> PedidoAdicional { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new FormaPagamentoMap());
            modelBuilder.ApplyConfiguration(new FuncionarioMap());
            modelBuilder.ApplyConfiguration(new ComandaMap());
            modelBuilder.ApplyConfiguration(new NotaFiscalMap());
            modelBuilder.ApplyConfiguration(new CupomClienteMap());
            modelBuilder.ApplyConfiguration(new ItemMap());
            modelBuilder.ApplyConfiguration(new PedidoMap());
            modelBuilder.ApplyConfiguration(new AdicionalMap());
            modelBuilder.ApplyConfiguration(new PedidoItemMap());
            modelBuilder.ApplyConfiguration(new PedidoAdicionalMap());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\;Database=ProjetoCafe;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        }
    }
}