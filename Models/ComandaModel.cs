using ProjetoCafe.DTOS;

namespace ProjetoCafe.Models

{
    public class ComandaModel
    {
        public int ComandaID { get; set; }
        public int NumeroComanda { get; set; }
        public int? Mesa { get; set; }
        public bool EstaAberta { get; set; }


        public ComandaModel(){ }

        public ComandaModel(ComandaDTO comandaDto)
        {
            NumeroComanda = comandaDto.NumeroComanda;
            Mesa = comandaDto.Mesa;
            EstaAberta = comandaDto.EstaAberta;
        }
    }
}