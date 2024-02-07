using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models
{
    public class NotaFiscalModel
    {
        private double _desconto;
        public int NotaFiscalID { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public double ValorTotal { get; set; }
        public bool TaxaServico { get; set; }
        public double Desconto
        {
            get
            {
                return _desconto;
            }
            set
            {
                if (value > 999.99)
                {
                    throw new CafeException("Desconto excede o valor permitido");
                }
                _desconto = value;
            }
        }
        public double ValorFinal { get; set; }
        public int ComandaID { get; set; }
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