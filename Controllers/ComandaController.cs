using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComandaController : ControllerBase
    {
        private readonly CafeDBContext _context;

        public ComandaController()
        {
            _context = new CafeDBContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var comandas = _context.Comanda.ToList();
                List<ComandaDTO> listComanda = new List<ComandaDTO>();

                foreach (var comanda in comandas)
                {
                    listComanda.Add(new ComandaDTO(comanda));
                }

                return Ok(listComanda);
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
                var comanda = _context.Comanda
                .FirstOrDefault(c => c.ComandaID == id);

                if (comanda == null)
                    return NotFound("Comanda não encontrada");

                return Ok(new ComandaDTO(comanda));
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

        [HttpGet("ExibirPedidos/{Id}")]
        public IActionResult GetPedidos(int Id)
        {
            try
            {
                var comada = _context.Comanda.FirstOrDefault(c => c.ComandaID == Id);
                if (comada == null)
                {
                    return NotFound($"Comanda com ID {Id} não encontrada");
                }
                var pedidos = _context.Pedido
                .Where(p => p.ComandaID == Id)
                .ToList();


                if (pedidos.Count == 0)
                    return BadRequest("Nenhum pedido adicionado");

                List<PedidoDTO> pedidosDTO = new List<PedidoDTO>();

                foreach (var p in pedidos)
                {
                    pedidosDTO.Add(new PedidoDTO((p)));
                }

                return Ok(pedidosDTO);
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
        public IActionResult Post([FromBody] ComandaDTO novaComanda)
        {
            try
            {
                var comandas = _context.Comanda
                .Where(
                    c => c.EstaAberta == true &&
                    c.NumeroComanda == novaComanda.NumeroComanda
                )
                .ToList();

                if (comandas.Count > 0)
                    return BadRequest("Comanda não disponivel");

                ComandaModel comanda = new ComandaModel(novaComanda);
                _context.Comanda.Add(comanda);
                _context.SaveChanges();

                novaComanda.setComandaId(comanda.ComandaID);
                novaComanda.EstaAberta = true;
                return Ok(novaComanda);
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
        public IActionResult Put(int id, [FromBody] ComandaDTO edicoes)
        {
            try
            {
                var comanda = _context.Comanda
                .FirstOrDefault(c => c.ComandaID == id);

                if (comanda == null)
                    return NotFound("Comanda não encontrada");

                comanda = edicoes.editComanda(comanda);

                _context.Entry(comanda).CurrentValues.SetValues(comanda);
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
                var comanda = _context.Comanda
                    .FirstOrDefault(c => c.ComandaID == id);

                if (comanda == null)
                {
                    return NotFound("Comanda não encontrada");
                }

                _context.Comanda.Remove(comanda);
                _context.SaveChanges();
                return Ok($"Comanda numero {comanda.NumeroComanda} excluida");
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