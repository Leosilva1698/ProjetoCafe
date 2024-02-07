using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models

{
    public class PedidoAdicionalModel
    {
        private int _Quantidade;
        public int PedidoID { get; set; }        
        public PedidoModel? Pedido { get; set; }
        public int AdicionalID { get; set; }
        public AdicionalModel? Adicional { get; set; }
        public int Quantidade { 
            get {
                return _Quantidade;
            } 
            set {
                if (value <= 0)
                {
                    throw new CafeException("Quantidade informada é inválida!");
                }
                _Quantidade = value;
            } 
        }
        public PedidoAdicionalModel() { }

        public PedidoAdicionalModel(PedidoAdicionalDTO pedidoAdicional)
        {
            PedidoID = pedidoAdicional.PedidoID;
            AdicionalID = pedidoAdicional.AdicionalID;
            Quantidade = pedidoAdicional.Quantidade;
        }
    }
}