using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class AdicionalDTO
    {
        public int AdicionalID { get; private set; }
        public string? Descricao { get; set; }
        public double Valor { get; set; }

        public AdicionalDTO() { }

        public AdicionalDTO(AdicionalModel adcional)
        {
            AdicionalID = adcional.AdicionalID;
            Descricao = adcional.Descricao;
            Valor = adcional.Valor;
        }

        public void setAdicionalId(int id)
        {
            AdicionalID = id;
        }

        public AdicionalModel editAdicional(AdicionalModel adicional)
        {
            setAdicionalId(adicional.AdicionalID);
            adicional.Descricao = Descricao;
            adicional.Valor = Valor;

            return adicional;
        }
    }
}