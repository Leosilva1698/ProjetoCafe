using Microsoft.AspNetCore.Mvc;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;

namespace ProjetoCafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdicionalController : ControllerBase
    {
        private readonly CafeDBContext _context;

        public AdicionalController()
        {   
            _context = new CafeDBContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var adicional = _context.Adicional.ToList();
            List<AdicionalDTO> listAdicionais = new List<AdicionalDTO>();

            foreach (var a in adicional)
            {
                listAdicionais.Add(new AdicionalDTO(a));
            }

            return Ok(listAdicionais);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var adicional = _context.Adicional
                .FirstOrDefault(a => a.AdicionalID == id);

            if (adicional == null)
                return NotFound();
        
            return Ok(new AdicionalDTO(adicional));
        }

        [HttpPost]
        public IActionResult Post([FromBody] AdicionalDTO novoAdicional)
        {
            AdicionalModel adicional = new AdicionalModel(novoAdicional);
            _context.Adicional.Add(adicional);
            _context.SaveChanges();
            
            novoAdicional.setAdicionalId(adicional.AdicionalID);
            return Ok(novoAdicional);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AdicionalDTO edicoes)
        {
            var adicional = _context.Adicional
                .FirstOrDefault(a => a.AdicionalID == id);

            if (adicional == null)
                return NotFound();
            
            adicional = edicoes.editAdicional(adicional);

            _context.Entry(adicional).CurrentValues.SetValues(adicional);
            _context.SaveChanges();
            return Ok(edicoes);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var adicional = _context.Adicional
                    .FirstOrDefault(a => a.AdicionalID == id);
                
                if (adicional == null)
                {
                    return NotFound();
                }

                _context.Adicional.Remove(adicional);
                _context.SaveChanges();
                return Ok();
        }
    }
}