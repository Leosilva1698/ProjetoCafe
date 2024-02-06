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
        private string? _telefone;
        private string? _email;
        private string? _CPF;
        public int ClienteID { get; set; }
        public string? Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Trim() == "")
                {
                    throw new CafeException("O nome do cliente é obrigatorio");
                }
                if (value.Length > 70)
                {
                    throw new CafeException("Nome do cliente excedeu o limete de 70 caracteres");
                }

                this._nome = value;
            }
        }
        public string? Telefone
        {
            get
            {
                return _telefone;
            }
            set
            {
                if (value != null)
                {
                    if (value.Length != 11)
                    {
                        throw new CafeException("Telefone invalido. Ex:(41999999999)");
                    }
                }
                this._telefone = value;
            }
        }
        public string? Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != null)
                {
                    if (value.Length > 50)
                    {
                        throw new CafeException("Email excedeu o limite de 50 caracteres");
                    }
                }
                this._email = value;
            }
        }
        public string? CPF
        {
            get
            {
                return _CPF;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Trim() == "")
                {
                    throw new CafeException("O CPF é obrigatorio");
                }
                if (value.Length != 11)
                {
                    throw new CafeException("CPF invalido. Ex(12345678912)");
                }

                this._CPF = value;
            }
        }

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