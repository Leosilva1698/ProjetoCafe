using ProjetoCafe.DTOS;

namespace ProjetoCafe.Models

{
    public class PedidoItemModel
    {
        public int PedidoID { get; set; }
        public PedidoModel? Pedido { get; set; }
        public int ItemID { get; set; }
        public ItemModel? Item { get; set; }
        public string? Observacao { get; set; }
        public int Quantidade { get; set; }

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