using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class FuncionarioDTO
    {
        public int FuncionarioID { get; private set; }
        public string? Nome { get; set; }

        public FuncionarioDTO(){ }

        public FuncionarioDTO(FuncionarioModel funcionario)
        {
            FuncionarioID = funcionario.FuncionarioID;
            Nome = funcionario.Nome;
        }

        public void setFuncionarioId(int id)
        {
            FuncionarioID = id;
        }

        public FuncionarioModel editFuncionario(FuncionarioModel funcionario)
        {
            setFuncionarioId(funcionario.FuncionarioID);
            funcionario.Nome = Nome;
            return funcionario;
        }
    }
}