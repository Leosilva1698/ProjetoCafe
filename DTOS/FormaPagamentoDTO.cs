using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class FormaPagamentoDTO
    {
        public int FormaPagamentoID { get; private set; }
        public string? Descricao { get; set; }

        public  FormaPagamentoDTO(){ }

        public  FormaPagamentoDTO(FormaPagamentoModel formaPagamento)
        {
            FormaPagamentoID = formaPagamento.FormaPagamentoID;
            Descricao = formaPagamento.Descricao;
        }

        public void setFormaPagamentoId(int id)
        {
            FormaPagamentoID = id;
        }

        public FormaPagamentoModel editFuncionario(FormaPagamentoModel formaPagamento)
        {
            setFormaPagamentoId(formaPagamento.FormaPagamentoID);
            formaPagamento.Descricao = Descricao;
            return formaPagamento;
        }
    }
}