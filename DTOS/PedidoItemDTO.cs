using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class PedidoItemDTO
    {
        public int PedidoID { get; set; }
        public int ItemID { get; set; }
        public string? Observacao { get; set; }
        public int Quantidade { get; set; }     

        public PedidoItemModel editPedidoItem(PedidoItemModel pedidoItem)
        {
            PedidoID = pedidoItem.PedidoID;
            ItemID = pedidoItem.ItemID;
            pedidoItem.Observacao = Observacao;
            pedidoItem.Quantidade = Quantidade;

            return pedidoItem; 
        }
    }
}