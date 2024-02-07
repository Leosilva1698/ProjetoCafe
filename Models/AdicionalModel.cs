using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models

{
    public class AdicionalModel
    {

        private string? _Descricao;
        private double _Valor;
        public int AdicionalID { get; set; }
        public string? Descricao
        {
            get
            {
                return _Descricao;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Trim() == "")
                {
                    throw new CafeException("Descrição é obrigatória");
                }
                else if (value.Length > 30)
                {
                    throw new CafeException("Descrição do adicional excedeu o limite de 30 caracteres");
                }

                _Descricao = value;
            }
        }
        public double Valor
        {
            get
            {
                return _Valor;
            }
            set
            {
                if (value <= 0 || value > 999.99)
                {
                    throw new CafeException("Valor inválido!");
                }

                _Valor = value;
            }
        }

        public AdicionalModel() { }
        public AdicionalModel(AdicionalDTO adicional)
        {
            Descricao = adicional.Descricao;
            Valor = adicional.Valor;
        }
    }
}