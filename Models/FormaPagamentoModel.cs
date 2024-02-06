using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models

{
    public class FormaPagamentoModel
    {
        private string? _Descricao;
        public int FormaPagamentoID { get; set; }
        public string? Descricao
        {
            get
            {
                return _Descricao;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Trim() == "")
                {
                    throw new CafeException("A descrição é obrigatoria");
                }
                if (value.Length > 20)
                {
                    throw new CafeException("Descrição excedeu o limete de 20 caracteres");
                }

                this._Descricao = value;
            }
        }


        public FormaPagamentoModel() { }

        public FormaPagamentoModel(FormaPagamentoDTO formaPagamento)
        {
            Descricao = formaPagamento.Descricao;
        }
    }
}