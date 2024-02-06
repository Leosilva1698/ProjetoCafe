namespace ProjetoCafe.Models
{
    public class CupomClienteModel
    {
        public int NotaFiscalID { get; set; }
        public NotaFiscalModel? NotaFiscal { get; set; }
        public int ClienteID { get; set; }
        public ClienteModel? Cliente { get; set; }
        public DateTime? DataValidade { get; set; }
        public DateTime? DataGeracao { get; set; }
        public double Valor { get; set; }
        public bool Valido { get; set; }

        public CupomClienteModel() { }

        public CupomClienteModel(int ClienteId, int NotaFiscalId, DateTime DataGeracao, DateTime DataValidade, double Valor, bool Valido)
        {
            this.ClienteID = ClienteId;
            this.NotaFiscalID = NotaFiscalId;
            this.DataGeracao = DataGeracao;
            this.DataValidade = DataValidade;
            this.Valor = Valor;
            this.Valido = Valido;
        }

    }
}