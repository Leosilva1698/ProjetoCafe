using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class NotaFiscalDTO
    {
        public int NotaFiscalID { get; private set; }
        public DateTime? DataHoraCriacao { get; private set; }
        public double? ValorTotal { get; private set; }
        public bool TaxaServico { get; set; }
        public double Desconto { get; set; }
        public double? ValorFinal { get; private set; }
        public int? ClienteID { get; set; }
        public int ComandaID  { get; set; }
        public int FormaPagamentoID { get; set; }

        public NotaFiscalDTO() { }

        public NotaFiscalDTO(NotaFiscalModel notaFiscal)
        {
            NotaFiscalID = notaFiscal.NotaFiscalID;
            DataHoraCriacao = notaFiscal.DataHoraCriacao;
            ValorTotal = notaFiscal.ValorTotal;
            TaxaServico = notaFiscal.TaxaServico;
            Desconto = notaFiscal.Desconto;
            ValorFinal = notaFiscal.ValorFinal;
            ComandaID = notaFiscal.ComandaID;
            FormaPagamentoID = notaFiscal.FormaPagamentoID;
        }

        public void setNotaFiscalValues(NotaFiscalModel notaFiscal)
        {
            NotaFiscalID = notaFiscal.NotaFiscalID;
            DataHoraCriacao = notaFiscal.DataHoraCriacao;
            ValorTotal = notaFiscal.ValorTotal;
            ValorFinal = notaFiscal.ValorFinal;
            Desconto = notaFiscal.Desconto;
        }
    }
}