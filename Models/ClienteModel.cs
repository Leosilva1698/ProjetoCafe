using System.Security.Cryptography;
using Azure.Messaging;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Models

{
    public class ClienteModel
    {
        private string? _nome;
        public int ClienteID { get; set; }
        public string? Nome {
            get 
            {
                return _nome;
            } 
            set 
            {
                if (string.IsNullOrEmpty(value) || value.Length > 70 || value.Trim() == "")
                {
                    throw new CafeException("Erro no nome");
                }
                this._nome = value;
            } 
        }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
        
        public ClienteModel() { }

        public ClienteModel(ClienteDTO cliente)
        {
            Nome = cliente.Nome;
            Telefone = cliente.Telefone;
            Email = cliente.Email;
            CPF = cliente.CPF;
        }
    }
}