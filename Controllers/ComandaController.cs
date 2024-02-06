using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;

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
            var comandas = _context.Comanda.ToList();
            List<ComandaDTO> listComanda = new List<ComandaDTO>();

            foreach (var comanda in comandas)
            {
                listComanda.Add(new ComandaDTO(comanda));
            }

            return Ok(listComanda);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var comanda = _context.Comanda
                .FirstOrDefault(c => c.ComandaID == id);

            if (comanda == null)
                return NotFound();            
            
            return Ok(new ComandaDTO(comanda));
        }

        [HttpGet("ExibirPedidos/{Id}")]
        public IActionResult GetPedidos(int Id)
        {
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

        [HttpPost]
        public IActionResult Post([FromBody] ComandaDTO novaComanda)
        {            
            var comandas = _context.Comanda
                .Where (
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
            return Ok(novaComanda);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ComandaDTO edicoes)
        {
            var comanda = _context.Comanda
                .FirstOrDefault(c => c.ComandaID == id);            

            if (comanda == null)
                return NotFound();

            comanda = edicoes.editComanda(comanda);
            
            _context.Entry(comanda).CurrentValues.SetValues(comanda);
            _context.SaveChanges();
            return Ok(edicoes);
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
                    return NotFound();
                }

                _context.Comanda.Remove(comanda);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}