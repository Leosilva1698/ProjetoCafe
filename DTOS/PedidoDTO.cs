using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class PedidoDTO
    {   
        public int PedidoID { get; private set; }
        public TimeOnly HoraFeito { get; private set; }
        public double? ValorTotal { get; private set; }
        public int FuncionarioID {  get; set; }
        public int ComandaID  { get; set; }

        public PedidoDTO() { }

        public PedidoDTO(PedidoModel pedido)
        {
            PedidoID = pedido.PedidoID;
            HoraFeito = pedido.HoraFeito;
            FuncionarioID = pedido.FuncionarioID;
            ComandaID = pedido.ComandaID;
            ValorTotal = pedido.ValorTotal;
        }

        public void setValues(PedidoModel pedido)
        {
            PedidoID = pedido.PedidoID;
            HoraFeito = pedido.HoraFeito;
        }

        public PedidoModel editPedido(PedidoModel pedido)
        {
            setValues(pedido);
            FuncionarioID = pedido.FuncionarioID;
            ComandaID = pedido.ComandaID;

            return pedido;
        }
    }
}