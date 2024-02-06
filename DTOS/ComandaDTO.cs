using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class ComandaDTO
    {
        public int ComandaID { get; private set;}
        public int NumeroComanda { get; set; }
        public int? Mesa { get; set; }
        public bool EstaAberta { get; set; }

        public ComandaDTO(){ }

        public ComandaDTO(ComandaModel comandaModel)
        {
            ComandaID = comandaModel.ComandaID;
            NumeroComanda = comandaModel.NumeroComanda;
            Mesa = comandaModel.Mesa;
            EstaAberta = comandaModel.EstaAberta;
        }

        public void setComandaId(int id)
        {
            ComandaID = id;
        }

        public ComandaModel editComanda(ComandaModel comanda)
        {
            setComandaId(comanda.ComandaID);
            comanda.NumeroComanda = NumeroComanda;
            comanda.Mesa = Mesa;
            comanda.EstaAberta = EstaAberta;

            return comanda;
        }
    }
}