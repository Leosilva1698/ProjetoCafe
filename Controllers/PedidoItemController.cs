using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;

namespace ProjetoCafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoItemController : ControllerBase
    {
        private readonly CafeDBContext _context;

        public PedidoItemController()
        {   
            _context = new CafeDBContext();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var pedido = _context.PedidoItem
                .Include(pi => pi.Pedido)
                .Select( pi => new { pi.Pedido!.PedidoID })
                .FirstOrDefault(pi => pi.PedidoID == id);

            if (pedido == null)
                return BadRequest("Sem itens adicionados");

            var itens = _context.PedidoItem
                .Include(pi => pi.Item)
                .Where(pi => pi.PedidoID == id)
                .Select( pi => new 
                    { pi.Item!.Nome, pi.Item.Valor, pi.Observacao, pi.Quantidade }
                )
                .ToList();

            return Ok(new {
                Pedido = pedido.PedidoID,
                Itens = itens
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] PedidoItemDTO novoPedidoItem)
        {   
 
                PedidoItemModel pedidoItem = new PedidoItemModel(novoPedidoItem);

                var pedido = _context.Pedido
                    .FirstOrDefault(p => p.PedidoID == novoPedidoItem.PedidoID)!;

                var item = _context.Item
                    .FirstOrDefault(i => i.ItemID == novoPedidoItem.ItemID)!;

                pedido.ValorTotal += item.Valor * pedidoItem.Quantidade;
                
                _context.PedidoItem.Add(pedidoItem);
                _context.Entry(pedido).CurrentValues.SetValues(pedido);
                _context.SaveChanges();
                
                return Ok(novoPedidoItem);
            
            
        }

        [HttpPut]
        public IActionResult Put([FromBody] PedidoItemDTO edicoes)
        {
            var pedidoItem = _context.PedidoItem
                .Include(pi => pi.Pedido)
                .Include(pi => pi.Item)
                .FirstOrDefault
                (
                    pi => pi.PedidoID == edicoes.PedidoID && 
                    pi.ItemID == edicoes.ItemID
                );
            
            if (pedidoItem == null)
                return NotFound();

            PedidoModel pedido = pedidoItem.Pedido!;
            ItemModel item = pedidoItem.Item!;

            pedido.ValorTotal -= item.Valor * pedidoItem.Quantidade;
            pedido.ValorTotal += item.Valor * edicoes.Quantidade;
            
            pedidoItem = edicoes.editPedidoItem(pedidoItem);

            _context.Entry(pedidoItem).CurrentValues.SetValues(pedidoItem);
            _context.Entry(pedido).CurrentValues.SetValues(pedido);
            _context.SaveChanges();
            return Ok(edicoes);
        }

        [HttpDelete]
        public IActionResult Delete(int idPedido, int idItem)
        {
            var pedidoItem = _context.PedidoItem
                .Include(pi => pi.Pedido)
                .Include(pi => pi.Item)
                .FirstOrDefault(pi => pi.PedidoID == idPedido && pi.ItemID == idItem);

            if (pedidoItem == null)
                return NotFound();

            PedidoModel pedido = pedidoItem.Pedido!;
            ItemModel item = pedidoItem.Item!;

            pedido.ValorTotal -= item.Valor * pedidoItem.Quantidade;

            _context.PedidoItem.Remove(pedidoItem);
            _context.Entry(pedido).CurrentValues.SetValues(pedido);
            _context.SaveChanges();
            return Ok();
        }
    }
}