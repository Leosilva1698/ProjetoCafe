using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;
using ProjetoCafe.Exceptions;
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
            try
            {
                var pedido = _context.Pedido
                .FirstOrDefault(p => p.PedidoID == idPedido);

                if (pedido == null)
                    return NotFound("Pedido não encontrado");

                var itens = _context.PedidoItem
                    .Include(pi => pi.Item)
                    .Where(pi => pi.PedidoID == idPedido)
                    .Select(pi => new
                    { pi.Item!.Nome, pi.Item.Valor, pi.Observacao, pi.Quantidade }
                    )
                    .ToList();

                var adicionais = _context.PedidoAdicional
                    .Include(pa => pa.Adicional)
                    .Where(pa => pa.PedidoID == idPedido)
                    .Select(pa => new
                    { pa.Adicional!.Descricao, pa.Adicional.Valor, pa.Quantidade }
                    )
                    .ToList();

                return Ok(new
                {
                    Pedido = new PedidoDTO(pedido),
                    Itens = itens,
                    Adicionais = adicionais
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
        public IActionResult Post([FromBody] PedidoDTO novoPedido)
        {
            try
            {
                var comanda = _context.Comanda.FirstOrDefault(c => c.ComandaID == novoPedido.ComandaID);
                if (comanda == null || comanda.EstaAberta == false)
                    return BadRequest("Comanda não foi criada ou ja esta fechada!");

                var funcionario = _context.Funcionario.FirstOrDefault(f => f.FuncionarioID == novoPedido.FuncionarioID);
                if (funcionario == null)
                    return BadRequest("Funcionario não criado");


                PedidoModel pedido = new PedidoModel(novoPedido);
                _context.Pedido.Add(pedido);
                _context.SaveChanges();

                novoPedido.setValues(pedido);
                return Ok(novoPedido);
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
        public IActionResult Put(int id, [FromBody] PedidoDTO edicoes)
        {
            try
            {
                var comanda = _context.Comanda.FirstOrDefault(c => c.ComandaID == edicoes.ComandaID);
                if (comanda == null || comanda.EstaAberta == false)
                    return BadRequest("Comanda não foi criada ou ja esta fechada!");

                var funcionario = _context.Funcionario.FirstOrDefault(f => f.FuncionarioID == edicoes.FuncionarioID);
                if (funcionario == null)
                    return BadRequest("Funcionario não criado");

                var pedido = _context.Pedido
                .FirstOrDefault(p => p.PedidoID == id);

                if (pedido == null)
                    return NotFound("Pedido não encontrado");

                pedido = edicoes.editPedido(pedido);

                _context.Entry(pedido).CurrentValues.SetValues(pedido);
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
                var pedido = _context.Pedido
                .FirstOrDefault(p => p.PedidoID == id);

                if (pedido == null)
                    return NotFound("Pedido não encontrado");

                _context.Pedido.Remove(pedido);
                _context.SaveChanges();
                return Ok($"Pedido numero {pedido.PedidoID} excluido");
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