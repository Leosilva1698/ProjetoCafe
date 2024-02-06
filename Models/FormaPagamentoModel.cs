using ProjetoCafe.DTOS;

namespace ProjetoCafe.Models

{
    public class FormaPagamentoModel
    {
        public int FormaPagamentoID { get; set; }
        public string? Descricao { get; set; }


        public FormaPagamentoModel() { }

        public FormaPagamentoModel(FormaPagamentoDTO formaPagamento)
        {
            Descricao = formaPagamento.Descricao;
        }
    }
}