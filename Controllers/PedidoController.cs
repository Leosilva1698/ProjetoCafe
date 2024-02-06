using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;

namespace ProjetoCafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly CafeDBContext _context;

        public PedidoController()
        {   
            _context = new CafeDBContext();
        }

        [HttpGet]
        public IActionResult GetById(int idPedido)
        {
            var pedido = _context.Pedido
                .FirstOrDefault(p => p.PedidoID == idPedido);
            
            if (pedido == null)
                return NotFound();

            var itens = _context.PedidoItem
                .Include(pi => pi.Item)
                .Where(pi => pi.PedidoID == idPedido)
                .Select( pi => new 
                    { pi.Item!.Nome, pi.Item.Valor, pi.Observacao, pi.Quantidade }
                )
                .ToList();
            
            var adicionais = _context.PedidoAdicional
                .Include(pa => pa.Adicional)
                .Where(pa => pa.PedidoID == idPedido)
                .Select( pa => new 
                    { pa.Adicional!.Descricao, pa.Adicional.Valor, pa.Quantidade }
                )
                .ToList();

            return Ok(new {
                Pedido = new PedidoDTO(pedido),
                Itens = itens,
                Adicionais = adicionais
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] PedidoDTO novoPedido)
        {
            var comanda = _context.Comanda.FirstOrDefault(c => c.ComandaID == novoPedido.ComandaID);

            if (comanda == null || comanda.EstaAberta == false)
                return BadRequest("Comanda não foi criada ou ja esta fechada!");

            PedidoModel pedido = new PedidoModel(novoPedido);
            _context.Pedido.Add(pedido);
            _context.SaveChanges();
            
            novoPedido.setValues(pedido);
            return Ok(novoPedido);
        }
    
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PedidoDTO edicoes)
        {
            var pedido = _context.Pedido
                .FirstOrDefault(p => p.PedidoID == id);

            if (pedido == null)
                return NotFound();
            
            pedido = edicoes.editPedido(pedido);

            _context.Entry(pedido).CurrentValues.SetValues(pedido);
            _context.SaveChanges();
            return Ok(edicoes);
        }
    
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pedido = _context.Pedido
                .FirstOrDefault(p => p.PedidoID == id);

            if (pedido == null)
                return NotFound();

            _context.Pedido.Remove(pedido);
            _context.SaveChanges();
            return Ok();
        }
    }
}