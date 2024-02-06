using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models

{
    public class ItemModel
    {
        private string? _Nome;
        private string? _Descricao;
        private string? _Tamanho;
        private double _Valor;
        public int ItemID { get; set; }
        public string? Nome { 
            get{
                return _Nome;
            } 
            set{
                if (string.IsNullOrEmpty(value) || value.Trim() == "")
                {
                    throw new CafeException("Nome é obrigatorio!");
                } else if (value.Length > 30 )
                {
                    throw new CafeException("Nome do item excedeu o limite de 30 caracteres");
                }
                _Nome = value;
            } 
        }
        public string? Descricao { 
            get{
                return _Descricao;
            } 
            set{
                if (value != null)
                {
                    if (value.Length > 45 )
                    {
                        throw new CafeException("Descrição do item excedeu o limite de 45 caracteres");
                    }
                }
                _Descricao = value;
            } 
        }
        public string? Tamanho { 
            get{
                return _Tamanho;
            } 
            set{
                if (value != null)
                {
                    if (value.Length > 1 )
                    {
                        throw new CafeException("Tamanho do item deve ser apenas a inicial. Ex: 'P', 'M' ou 'G'");
                    }
                }
                _Tamanho = value;
            }
        }
        public double Valor { 
            get {
                return _Valor;
            } 
            set {
                if (value <= 0 || value > 999.99)
                {
                    throw new CafeException("Valor inválido!");
                }

                _Valor = value;
            } 
        }

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