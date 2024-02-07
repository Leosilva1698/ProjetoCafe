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
            try
            {
                 var pedido = _context.Pedido.FirstOrDefault(p => p.PedidoID == id);

                if (pedido == null)
                {
                    return NotFound("Pedido não encontrado!");
                }

                var pedidoAdicional = _context.PedidoAdicional
                    .Include(pa => pa.Pedido) 
                    .Select( pa => new { pa.Pedido!.PedidoID, pa.Pedido.ValorTotal})
                    .FirstOrDefault(pa => pa.PedidoID == id);

                if (pedidoAdicional == null)
                    return BadRequest("Sem adicionais incluidos");

                var adicionais = _context.PedidoAdicional
                    .Include(pa => pa.Adicional)
                    .Where(pa => pa.PedidoID == id)
                    .Select( pa => new 
                        { pa.Adicional!.Descricao, pa.Adicional.Valor, pa.Quantidade }
                    )
                    .ToList();

                return Ok(new {pedidoAdicional, adicionais});
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
        public IActionResult Post([FromBody] PedidoAdicionalDTO novoPedidoAdicional)
        {   
            try
            {
                var pedido = _context.Pedido
                    .FirstOrDefault(p => p.PedidoID == novoPedidoAdicional.PedidoID)!;

                if (pedido == null)
                {
                    return NotFound("Pedido não encontrado");
                }

                var adicional = _context.Adicional
                    .FirstOrDefault(i => i.AdicionalID == novoPedidoAdicional.AdicionalID)!;

                if (adicional == null)
                {
                    return NotFound("Adicional não encontrado");
                }

                PedidoAdicionalModel pedidoAdicional = new PedidoAdicionalModel(novoPedidoAdicional);

                pedido.ValorTotal += adicional.Valor * pedidoAdicional.Quantidade;
                
                _context.PedidoAdicional.Add(pedidoAdicional);
                _context.Entry(pedido).CurrentValues.SetValues(pedido);
                _context.SaveChanges();
                
                return Ok(novoPedidoAdicional);
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
                return BadRequest("Erro ao persistir no Banco de Dados. Certifique-se que o adicional já não foi adicionado ao pedido!");
            }
            catch (Exception e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Sistema fora do Ar!");
            }
            
        }

        [HttpPut]
        public IActionResult Put([FromBody] PedidoAdicionalDTO edicoes)
        {
            try
            {
                var pedido = _context.Pedido
                    .FirstOrDefault(p => p.PedidoID == edicoes.PedidoID)!;

                if (pedido == null)
                {
                    return NotFound("Pedido não encontrado");
                }

                var adicional = _context.Adicional
                    .FirstOrDefault(i => i.AdicionalID == edicoes.AdicionalID)!;

                if (adicional == null)
                {
                    return NotFound("Adicional não encontrado");
                }

                var pedidoAdicional = _context.PedidoAdicional
                    .FirstOrDefault
                    (
                        pa => pa.PedidoID == edicoes.PedidoID && 
                        pa.AdicionalID == edicoes.AdicionalID
                    );
                
                if (pedidoAdicional == null)
                    return BadRequest($"Pedido não tem o adicional {adicional.Descricao}");

                pedido.ValorTotal -= adicional.Valor * pedidoAdicional.Quantidade;
                pedido.ValorTotal += adicional.Valor * edicoes.Quantidade;
                
                pedidoAdicional = edicoes.editPedidoAdicional(pedidoAdicional);

                _context.Entry(pedidoAdicional).CurrentValues.SetValues(pedidoAdicional);
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
        public IActionResult Delete(int idPedido, int idAdicional)
        {
            try
            {
                var pedido = _context.Pedido
                    .FirstOrDefault(p => p.PedidoID == idPedido)!;

                if (pedido == null)
                {
                    return NotFound("Pedido não encontrado");
                }

                var adicional = _context.Adicional
                    .FirstOrDefault(i => i.AdicionalID == idAdicional)!;

                if (adicional == null)
                {
                    return NotFound("Adicional não encontrado");
                }
                var pedidoAdicional = _context.PedidoAdicional
                    .FirstOrDefault(pa => pa.PedidoID == idPedido && pa.AdicionalID == idAdicional);

                if (pedidoAdicional == null)
                    return NotFound($"Pedido não tem o adicional {adicional.Descricao}");

                pedido.ValorTotal -= adicional.Valor * pedidoAdicional.Quantidade;

                _context.PedidoAdicional.Remove(pedidoAdicional);
                _context.Entry(pedido).CurrentValues.SetValues(pedido);
                _context.SaveChanges();
                return Ok($"Adicional {adicional.Descricao} excluido do pedido {pedido.PedidoID}");
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