using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;

namespace ProjetoCafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoAdicionalController : ControllerBase
    {
         private readonly CafeDBContext _context;

        public PedidoAdicionalController()
        {   
            _context = new CafeDBContext();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var pedido = _context.PedidoAdicional
                .Include(pa => pa.Pedido) 
                .Select( pa => new { pa.Pedido!.PedidoID})
                .FirstOrDefault(pa => pa.PedidoID == id);

            if (pedido == null)
                return BadRequest("Sem adicionais incluidos");

            var adicionais = _context.PedidoAdicional
                .Include(pa => pa.Adicional)
                .Where(pa => pa.PedidoID == id)
                .Select( pa => new 
                    { pa.Adicional!.Descricao, pa.Adicional.Valor, pa.Quantidade }
                )
                .ToList();

            return Ok(new {pedido, adicionais});
        }

        [HttpPost]
        public IActionResult Post([FromBody] PedidoAdicionalDTO novoPedidoAdicional)
        {   
            PedidoAdicionalModel pedidoAdicional = new PedidoAdicionalModel(novoPedidoAdicional);

            var pedido = _context.Pedido
                .FirstOrDefault(p => p.PedidoID == novoPedidoAdicional.PedidoID)!;

            var adicional = _context.Adicional
                .FirstOrDefault(i => i.AdicionalID == novoPedidoAdicional.AdicionalID)!;

            pedido.ValorTotal += adicional.Valor * pedidoAdicional.Quantidade;
            
            _context.PedidoAdicional.Add(pedidoAdicional);
            _context.Entry(pedido).CurrentValues.SetValues(pedido);
            _context.SaveChanges();
            
            return Ok(novoPedidoAdicional);
        }

        [HttpPut]
        public IActionResult Put([FromBody] PedidoAdicionalDTO edicoes)
        {
            var pedidoAdicional = _context.PedidoAdicional
                .Include(pa => pa.Pedido)
                .Include(pa => pa.Adicional)
                .FirstOrDefault
                (
                    pa => pa.PedidoID == edicoes.PedidoID && 
                    pa.AdicionalID == edicoes.AdicionalID
                );
            
            if (pedidoAdicional == null)
                return NotFound();

            PedidoModel pedido = pedidoAdicional.Pedido!;
            AdicionalModel adicional = pedidoAdicional.Adicional!;

            pedido.ValorTotal -= adicional.Valor * pedidoAdicional.Quantidade;
            pedido.ValorTotal += adicional.Valor * edicoes.Quantidade;
            
            pedidoAdicional = edicoes.editPedidoAdicional(pedidoAdicional);

            _context.Entry(pedidoAdicional).CurrentValues.SetValues(pedidoAdicional);
            _context.Entry(pedido).CurrentValues.SetValues(pedido);
            _context.SaveChanges();
            return Ok(edicoes);
        }

        [HttpDelete]
        public IActionResult Delete(int idPedido, int idItem)
        {
            var pedidoAdicional = _context.PedidoAdicional
                .Include(pa => pa.Pedido)
                .Include(pa => pa.Adicional)
                .FirstOrDefault(pa => pa.PedidoID == idPedido && pa.AdicionalID == idItem);

            if (pedidoAdicional == null)
                return NotFound();

            PedidoModel pedido = pedidoAdicional.Pedido!;
            AdicionalModel adicional = pedidoAdicional.Adicional!;

            pedido.ValorTotal -= adicional.Valor * pedidoAdicional.Quantidade;

            _context.PedidoAdicional.Remove(pedidoAdicional);
            _context.Entry(pedido).CurrentValues.SetValues(pedido);
            _context.SaveChanges();
            return Ok();
        }
    }
}