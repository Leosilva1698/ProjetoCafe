using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;
using ProjetoCafe.Exceptions;
using System.ComponentModel;

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
            try
            {
                var clientes = _context.Cliente.ToList();
                List<ClienteDTO> listClientes = new List<ClienteDTO>();

                foreach (var c in clientes)
                {
                    listClientes.Add(new ClienteDTO(c));
                }

                return Ok(listClientes);
            }
            catch (InvalidOperationException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Erro ao se comunicar com o Banco de Dados");
            }
            catch (Exception e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Sistema fora do Ar!");
            }

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var cliente = _context.Cliente
                    .FirstOrDefault(c => c.ClienteID == id);

                if (cliente == null)
                    return NotFound("Cliente não encontrado");

                return Ok(new ClienteDTO(cliente));
            }
            catch (InvalidOperationException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Erro ao se comunicar com o Banco de Dados");
            }
            catch (Exception e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Sistema fora do Ar!");
            }

        }

        [HttpGet("VerCupons/{id}")]
        public IActionResult visualizarCupom(int id)
        {
            try
            {
                var cliente = _context.Cliente
                .FirstOrDefault(c => c.ClienteID == id);

                if (cliente == null)
                    return NotFound("Cliente não encontrado");

                var cupons = _context.CupomCliente
                    .Include(c => c.Cliente)
                    .Where(c => c.ClienteID == id && c.Valido == true)
                    .ToList();
                DateTime dataAtual = DateTime.Now;
                List<CupomClienteDTO> ListaCupom = new List<CupomClienteDTO>();
                foreach (var cupom in cupons)
                {
                    if (cupom.DataValidade < dataAtual)
                    {
                        cupom.Valido = false;
                        _context.Entry(cupom).CurrentValues.SetValues(cupom);
                        cupons.Remove(cupom);
                    }
                    else
                    {
                        ListaCupom.Add(new CupomClienteDTO(cupom));
                    }
                }
                _context.SaveChanges();
                return Ok(new
                {
                    Cliente = cliente.Nome,
                    Cupons = ListaCupom
                });
            }
            catch (InvalidOperationException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Erro ao se comunicar com o Banco de Dados");
            }
            catch (Exception e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Sistema fora do Ar!");
            }


        }

        [HttpPost]
        public IActionResult Post([FromBody] ClienteDTO novoCliente)
        {
            try
            {
                ClienteModel cliente = new ClienteModel(novoCliente);
                _context.Cliente.Add(cliente);
                _context.SaveChanges();

                novoCliente.setClienteId(cliente.ClienteID);
                return Ok(novoCliente);
            }
            catch (InvalidOperationException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Erro ao se comunicar com o Banco de Dados");
            }
            catch (FormatException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Formato inválido");
            }
            catch (CafeException e)
            {
                return BadRequest(e.avisoErro);
            }
            catch (Exception e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Sistema fora do Ar!");
            }

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ClienteDTO edicoes)
        {
            try
            {
                var cliente = _context.Cliente
                    .FirstOrDefault(c => c.ClienteID == id);

                if (cliente == null)
                    return NotFound("Cliente não encontrado");

                cliente = edicoes.editCliente(cliente);

                _context.Entry(cliente).CurrentValues.SetValues(cliente);
                _context.SaveChanges();
                return Ok(edicoes);
            }
            catch (InvalidOperationException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Erro ao se comunicar com o Banco de Dados");
            }
            catch (FormatException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Formato inválido");
            }
            catch (CafeException e)
            {
                return BadRequest(e.avisoErro);
            }
            catch (Exception e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Sistema fora do Ar!");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var cliente = _context.Cliente
                    .FirstOrDefault(c => c.ClienteID == id);

                if (cliente == null)
                {
                    return NotFound("Cliente não encontrado");
                }

                _context.Cliente.Remove(cliente);
                _context.SaveChanges();
                return Ok($"Cliente {cliente.Nome} excluido");
            }
            catch (InvalidOperationException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Erro ao se comunicar com o Banco de Dados");
            }
            catch (Exception e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Sistema fora do Ar!");
            }
        }
    }
}