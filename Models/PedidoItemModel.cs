using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models

{
    public class PedidoItemModel
    {
        private string? _Observacao;
        private int _Quantidade;
        public int PedidoID { get; set; }
        public PedidoModel? Pedido { get; set; }
        public int ItemID { get; set; }
        public ItemModel? Item { get; set; }
        public string? Observacao { 
            get {
                return _Observacao;
            } 
            set {
                if (value != null)
                {
                    if (value.Length > 70)
                    {
                        throw new CafeException("Observação do pedido excedeu o limite de 70 caracteres!");
                    }
                }
                _Observacao = value;
            } 
        }
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

        public PedidoItemModel() { }

        public PedidoItemModel(PedidoItemDTO pedidoItem)
        {
            PedidoID = pedidoItem.PedidoID;
            ItemID = pedidoItem.ItemID;
            Observacao = pedidoItem.Observacao;
            Quantidade = pedidoItem.Quantidade;
        }
    }
}