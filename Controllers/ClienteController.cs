using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;

namespace ProjetoCafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly CafeDBContext _context;

        public ClienteController()
        {   
            _context = new CafeDBContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var clientes = _context.Cliente.ToList();
            List<ClienteDTO> listClientes = new List<ClienteDTO>();

            foreach (var c in clientes)
            {
                listClientes.Add(new ClienteDTO(c));
            }

            return Ok(listClientes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cliente = _context.Cliente
                .FirstOrDefault(c => c.ClienteID == id);

            if (cliente == null)
                return NotFound();
        
            return Ok(new ClienteDTO(cliente));
        }

        [HttpGet("VerCupons/{id}")]
        public IActionResult visualizarCupom(int id)
        {
            var cliente = _context.CupomCliente
                .Include(c => c.Cliente)
                .FirstOrDefault(c => c.ClienteID == id);

            if (cliente == null)
                return NotFound();

            var cupons = _context.CupomCliente
                .Include(c => c.Cliente)
                .Where(c => c.ClienteID == id && c.Valido == true)
                .Select(c => new { c.Valor, c.NotaFiscalID })
                .ToList();

            return Ok(new {
                Cliente = cliente.Cliente!.Nome,
                Cupons = cupons
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClienteDTO novoCliente)
        {
            ClienteModel cliente = new ClienteModel(novoCliente);
            _context.Cliente.Add(cliente);
            _context.SaveChanges();
            
            novoCliente.setClienteId(cliente.ClienteID);
            return Ok(novoCliente);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ClienteDTO edicoes)
        {
            var cliente = _context.Cliente
                .FirstOrDefault(c => c.ClienteID == id);

            if (cliente == null)
                return NotFound();
            
            cliente = edicoes.editCliente(cliente);

            _context.Entry(cliente).CurrentValues.SetValues(cliente);
            _context.SaveChanges();
            return Ok(edicoes);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cliente = _context.Cliente
                .FirstOrDefault(c => c.ClienteID == id);
            
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Cliente.Remove(cliente);
            _context.SaveChanges();
            return Ok();
        }
    }
}