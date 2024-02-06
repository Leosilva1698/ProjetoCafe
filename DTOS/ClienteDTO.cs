using ProjetoCafe.Models;

namespace ProjetoCafe.DTOS
{
    public class ClienteDTO
    {
        public int ClienteID { get; private set; }
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }

        public ClienteDTO() { }

        public ClienteDTO(ClienteModel cliente)
        {
            ClienteID = cliente.ClienteID;
            Nome = cliente.Nome;
            Telefone = cliente.Telefone;
            Email = cliente.Email;
            CPF = cliente.CPF;
        }

        public void setClienteId(int id)
        {
            ClienteID = id;
        }

        public ClienteModel editCliente(ClienteModel cliente)
        {
            setClienteId(cliente.ClienteID);
            cliente.Nome = Nome;
            cliente.Telefone = Telefone;
            cliente.Email = Email;
            cliente.CPF = CPF;

            return cliente;
        }
    }
}