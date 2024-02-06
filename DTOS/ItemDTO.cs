using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class ItemDTO
    {
        public int ItemID { get; private set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Tamanho { get; set; }
        public double Valor { get; set; }
    
        public ItemDTO() { }

        public ItemDTO(ItemModel item)
        {
            ItemID = item.ItemID;
            Nome = item.Nome;
            Descricao = item.Descricao;
            Tamanho = item.Tamanho;
            Valor = item.Valor;
        }

        public void setItemId(int id)
        {
            ItemID = id;
        }
        
        public ItemModel editItem(ItemModel item)
        {
            setItemId(item.ItemID);
            item.Nome = Nome;
            item.Descricao = Descricao;
            item.Tamanho = Tamanho;
            item.Valor = Valor;
            return item;
        }
    }

}