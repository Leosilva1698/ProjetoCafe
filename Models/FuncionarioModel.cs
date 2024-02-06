using ProjetoCafe.DTOS;

namespace ProjetoCafe.Models

{
    public class FuncionarioModel
    {
        public int FuncionarioID { get; set; }
        public string? Nome { get; set; }

        public FuncionarioModel(){ }

        public FuncionarioModel(FuncionarioDTO funcionario)
        {
            Nome = funcionario.Nome;
        }
    }
}