using ProjetoCafe.DTOS;

namespace ProjetoCafe.Models

{
    public class PedidoAdicionalModel
    {
        public int PedidoID { get; set; }        
        public PedidoModel? Pedido { get; set; }
        public int AdicionalID { get; set; }
        public AdicionalModel? Adicional { get; set; }
        public int Quantidade { get; set; }

        public PedidoAdicionalModel() { }

        public PedidoAdicionalModel(PedidoAdicionalDTO pedidoAdicional)
        {
            PedidoID = pedidoAdicional.PedidoID;
            AdicionalID = pedidoAdicional.AdicionalID;
            Quantidade = pedidoAdicional.Quantidade;
        }
    }
}