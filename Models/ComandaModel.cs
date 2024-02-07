using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models

{
    public class ComandaModel
    {
        private int _numeroComanda;
        public int ComandaID { get; set; }
        public int? Mesa { get; set; }
        public bool EstaAberta { get; set; }
        public int NumeroComanda
        {
            get
            {
                return _numeroComanda;
            }
            set
            {
                if (value == 0)
                {
                    throw new CafeException("O numero da comanda é obrigatorio e diferente de zero");
                }

                this._numeroComanda = value;
            }
        }

        public ComandaModel() { }

        public ComandaModel(ComandaDTO comandaDto)
        {
            NumeroComanda = comandaDto.NumeroComanda;
            Mesa = comandaDto.Mesa;
            EstaAberta = true;
        }
    }
}