using Microsoft.AspNetCore.Mvc;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;
using ProjetoCafe.Models;

namespace ProjetoCafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly CafeDBContext _context;

        public ItemController()
        {   
            _context = new CafeDBContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var item = _context.Item.ToList();
                List<ItemDTO> listItens = new List<ItemDTO>();

                foreach (var i in item)
                {
                    listItens.Add(new ItemDTO(i));
                }

                return Ok(listItens);               
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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var item = _context.Item
                .FirstOrDefault(i => i.ItemID == id);

                if (item == null)
                    return NotFound("Item não encontrado!");
            
                return Ok(new ItemDTO(item));
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
        public IActionResult Post([FromBody] ItemDTO novoItem)
        {
            try
            {
                ItemModel item = new ItemModel(novoItem);
                _context.Item.Add(item);
                _context.SaveChanges();
                
                novoItem.setItemId(item.ItemID);
                return Ok(novoItem);
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ItemDTO edicoes)
        {
            try
            {
                var item = _context.Item
                .FirstOrDefault(i => i.ItemID == id);

                if (item == null)
                    return NotFound("Item não encontrado");
                
                item = edicoes.editItem(item);

                _context.Entry(item).CurrentValues.SetValues(item);
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var item = _context.Item
                    .FirstOrDefault(i => i.ItemID == id);
                
                if (item == null)
                {
                    return NotFound("Item não encontrado");
                }

                _context.Item.Remove(item);
                _context.SaveChanges();
                return Ok($"Item {item.Nome} excluido");
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