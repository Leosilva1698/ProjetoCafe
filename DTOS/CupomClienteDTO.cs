using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class CupomClienteDTO
    {
        public int NotaFiscalID { get; set; }
        public int ClienteID { get; set; }
        public DateTime? DataValidade { get; set; }
        public DateTime? DataGeracao { get; set; }
        public double Valor { get; set; }
        public bool Valido { get; set; }

        public CupomClienteDTO() { }

        public CupomClienteDTO(CupomClienteModel cupomCliente)
        {
            NotaFiscalID = cupomCliente.NotaFiscalID;
            ClienteID = cupomCliente.ClienteID;
            DataGeracao = cupomCliente.DataGeracao;
            DataValidade = cupomCliente.DataValidade;
            Valor = cupomCliente.Valor;
            Valido = cupomCliente.Valido;
        }

    }
}