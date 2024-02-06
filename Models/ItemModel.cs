using ProjetoCafe.DTOS;

namespace ProjetoCafe.Models

{
    public class ItemModel
    {
        public int ItemID { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Tamanho { get; set; }
        public double Valor { get; set; }

        public ItemModel() { }

        public ItemModel(ItemDTO item)
        {
            Nome = item.Nome;
            Descricao = item.Descricao;
            Tamanho = item.Tamanho;
            Valor = item.Valor;
        }
    }
}