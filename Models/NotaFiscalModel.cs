using ProjetoCafe.DTOS;

namespace ProjetoCafe.Models
{
    public class NotaFiscalModel
    {
        public int NotaFiscalID { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public double ValorTotal { get; set; }
        public bool TaxaServico { get; set; }
        public double Desconto { get; set; }
        public double ValorFinal { get; set; }
        public int ComandaID  { get; set; }
        public ComandaModel? Comanda { get; set; }
        public int FormaPagamentoID { get; set; }
        public FormaPagamentoModel? FormaPagamento { get; set; }

        public NotaFiscalModel() { }

        public NotaFiscalModel(NotaFiscalDTO notaFiscal)
        {
            DataHoraCriacao = DateTime.Now;
            ValorTotal = 0;
            TaxaServico = notaFiscal.TaxaServico;
            Desconto = notaFiscal.Desconto;
            ValorFinal = 0;
            ComandaID = notaFiscal.ComandaID;
            FormaPagamentoID = notaFiscal.FormaPagamentoID;
        }

    }
}