using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models

{
    public class FuncionarioModel
    {
        private string? _Nome;
        public int FuncionarioID { get; set; }
        public string? Nome
        {
            get
            {
                return _Nome;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Trim() == "")
                {
                    throw new CafeException("Nome do Funcionario é obrigatorio");
                }
                else if (value.Length > 60)
                {
                    throw new CafeException("Nome do Funcionario excedeu o limete de 60 caracteres");
                }
                _Nome = value;
            }
        }

        public FuncionarioModel() { }

        public FuncionarioModel(FuncionarioDTO funcionario)
        {
            Nome = funcionario.Nome;
        }
    }
}