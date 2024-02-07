using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models

{
    public class PedidoModel
    {
        private double _ValorTotal;
        public int PedidoID { get; set; }
        public TimeOnly HoraFeito { get; set; }
        public double ValorTotal
        {
            get
            {
                return _ValorTotal;
            }
            set
            {
                if (value > 999.99)
                {
                    throw new CafeException("Valor do pedido excedeu o limite de valor, crie outro pedido");
                }

                _ValorTotal = value;
            }
        }
        public int FuncionarioID { get; set; }
        public FuncionarioModel? Funcionario { get; set; }
        public int ComandaID { get; set; }
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