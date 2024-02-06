using Microsoft.AspNetCore.Mvc;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
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
            var item = _context.Item.ToList();
            List<ItemDTO> listItens = new List<ItemDTO>();

            foreach (var i in item)
            {
                listItens.Add(new ItemDTO(i));
            }

            return Ok(listItens);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.Item
                .FirstOrDefault(i => i.ItemID == id);

            if (item == null)
                return NotFound();
        
            return Ok(new ItemDTO(item));
        }

        [HttpPost]
        public IActionResult Post([FromBody] ItemDTO novoItem)
        {
            ItemModel item = new ItemModel(novoItem);
            _context.Item.Add(item);
            _context.SaveChanges();
            
            novoItem.setItemId(item.ItemID);
            return Ok(novoItem);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ItemDTO edicoes)
        {
            var item = _context.Item
                .FirstOrDefault(i => i.ItemID == id);

            if (item == null)
                return NotFound();
            
            item = edicoes.editItem(item);

            _context.Entry(item).CurrentValues.SetValues(item);
            _context.SaveChanges();
            return Ok(edicoes);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Item
                    .FirstOrDefault(i => i.ItemID == id);
                
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            _context.SaveChanges();
            return Ok();
        }
    }
}