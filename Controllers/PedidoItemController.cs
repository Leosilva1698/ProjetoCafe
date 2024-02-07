using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;
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
            try
            {
                var pedido = _context.Pedido.FirstOrDefault(p => p.PedidoID == id);

                if (pedido == null)
                {
                    return NotFound("Pedido não encontrado!");
                }

                var pedidoItem = _context.PedidoItem
                .Include(pi => pi.Pedido)
                .Select( pi => new { pi.Pedido!.PedidoID , pi.Pedido.ValorTotal})
                .FirstOrDefault(pi => pi.PedidoID == id);

                if (pedidoItem == null)
                    return Ok("Sem itens adicionados");

                var itens = _context.PedidoItem
                    .Include(pi => pi.Item)
                    .Where(pi => pi.PedidoID == id)
                    .Select( pi => new 
                        { pi.Item!.Nome, pi.Item.Valor, pi.Observacao, pi.Quantidade }
                    )
                    .ToList();

                return Ok(new {
                    Pedido = pedidoItem,
                    Itens = itens
                });
            }
            catch(InvalidOperationException e)
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
        public IActionResult Post([FromBody] PedidoItemDTO novoPedidoItem)
        {   
            try
            {
                var pedido = _context.Pedido
                    .FirstOrDefault(p => p.PedidoID == novoPedidoItem.PedidoID)!;

                if (pedido == null)
                {
                    return NotFound("Pedido não encontrado");
                }

                var item = _context.Item
                    .FirstOrDefault(i => i.ItemID == novoPedidoItem.ItemID)!;

                if (item == null)
                {
                    return NotFound("Item não encontrado");
                }

                PedidoItemModel pedidoItem = new PedidoItemModel(novoPedidoItem);

                pedido.ValorTotal += item.Valor * pedidoItem.Quantidade;
                
                _context.PedidoItem.Add(pedidoItem);
                _context.Entry(pedido).CurrentValues.SetValues(pedido);
                _context.SaveChanges();
                
                return Ok(novoPedidoItem);
            }
            catch(InvalidOperationException e)
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
            catch (DbUpdateException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Erro ao persistir no Banco de Dados. Certifique-se que o item já não foi adicionado ao pedido!");
            }
            catch (Exception e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Sistema fora do Ar!");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] PedidoItemDTO edicoes)
        {

            try
            {
                var pedido = _context.Pedido
                .FirstOrDefault(p => p.PedidoID == edicoes.PedidoID)!;

                if (pedido == null)
                {
                    return NotFound("Pedido não encontrado");
                }

                var item = _context.Item
                    .FirstOrDefault(i => i.ItemID == edicoes.ItemID)!;

                if (item == null)
                {
                    return NotFound("Item não encontrado");
                }

                var pedidoItem = _context.PedidoItem
                    .FirstOrDefault
                    (
                        pi => pi.PedidoID == edicoes.PedidoID && 
                        pi.ItemID == edicoes.ItemID
                    );
                
                if (pedidoItem == null)
                {
                    return BadRequest($"Pedido não tem o item: {item.Nome}");
                }

                pedido.ValorTotal -= item.Valor * pedidoItem.Quantidade;
                pedido.ValorTotal += item.Valor * edicoes.Quantidade;
                
                pedidoItem = edicoes.editPedidoItem(pedidoItem);

                _context.Entry(pedidoItem).CurrentValues.SetValues(pedidoItem);
                _context.Entry(pedido).CurrentValues.SetValues(pedido);
                _context.SaveChanges();
                return Ok(edicoes);
            }
            catch(InvalidOperationException e)
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

        [HttpDelete]
        public IActionResult Delete(int idPedido, int idItem)
        {
            try
            {
                var pedido = _context.Pedido
                    .FirstOrDefault(p => p.PedidoID == idPedido)!;

                if (pedido == null)
                {
                    return NotFound("Pedido não encontrado");
                }

                var item = _context.Item
                    .FirstOrDefault(i => i.ItemID == idItem)!;

                if (item == null)
                {
                    return NotFound("Item não encontrado");
                }

                var pedidoItem = _context.PedidoItem
                    .Include(pi => pi.Pedido)
                    .Include(pi => pi.Item)
                    .FirstOrDefault(pi => pi.PedidoID == idPedido && pi.ItemID == idItem);

                if (pedidoItem == null)
                    return NotFound($"Pedido não tem o item: {item.Nome}");

                pedido.ValorTotal -= item.Valor * pedidoItem.Quantidade;

                _context.PedidoItem.Remove(pedidoItem);
                _context.Entry(pedido).CurrentValues.SetValues(pedido);
                _context.SaveChanges();
                return Ok($"Item {item.Nome} excluido do pedido {pedido.PedidoID}");
            }
            catch(InvalidOperationException e)
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