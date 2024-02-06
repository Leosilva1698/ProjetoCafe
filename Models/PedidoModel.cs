using ProjetoCafe.DTOS;

namespace ProjetoCafe.Models

{
    public class PedidoModel
    {
        public int PedidoID { get; set; }
        public TimeOnly HoraFeito { get; set; }
        public double ValorTotal { get; set; }
        public int FuncionarioID { get; set; }
        public FuncionarioModel? Funcionario { get; set; }
        public int ComandaID  { get; set; }
        public ComandaModel? Comanda { get; set; }
        
        public PedidoModel() { }

        public PedidoModel(PedidoDTO pedido)
        {
            HoraFeito = TimeOnly.FromDateTime(DateTime.Now);
            FuncionarioID = pedido.FuncionarioID;
            ComandaID = pedido.ComandaID;
            ValorTotal = 0;
        }
    }
}