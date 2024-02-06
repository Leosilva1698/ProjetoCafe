using ProjetoCafe.DTOS;

namespace ProjetoCafe.Models

{
    public class AdicionalModel
    {
        public int AdicionalID { get; set; }
        public string? Descricao { get; set; }
        public double Valor { get; set; }

        public AdicionalModel() { }
        public AdicionalModel(AdicionalDTO adicional)
        {
            Descricao = adicional.Descricao;
            Valor = adicional.Valor;
        }
    }
}