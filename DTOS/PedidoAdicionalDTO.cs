using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class PedidoAdicionalDTO
    {
        public int PedidoID { get; set; }        
        public int AdicionalID { get; set; }
        public int Quantidade { get; set; }

        public PedidoAdicionalModel editPedidoAdicional(PedidoAdicionalModel pedidoAdicional)
        {
            PedidoID = pedidoAdicional.PedidoID;
            AdicionalID = pedidoAdicional.AdicionalID;
            pedidoAdicional.Quantidade = Quantidade;

            return pedidoAdicional;
        }
    }
}