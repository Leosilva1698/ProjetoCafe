using Microsoft.AspNetCore.Mvc;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;

namespace ProjetoCafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormaPagamentoController : ControllerBase
    {
        private readonly CafeDBContext _context;

        public FormaPagamentoController()
        {   
            _context = new CafeDBContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var formasPagamento = _context.FormaPagamento.ToList();
            List<FormaPagamentoDTO> listFormaPagemento = new List<FormaPagamentoDTO>();

            foreach (var fp in formasPagamento)
            {
                listFormaPagemento.Add(new FormaPagamentoDTO(fp));
            }

            return Ok(listFormaPagemento);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var formaPagamento = _context.FormaPagamento
                .FirstOrDefault(fp => fp.FormaPagamentoID == id);

            if (formaPagamento == null)
                return NotFound();
        
            return Ok(new FormaPagamentoDTO(formaPagamento));
        }

        [HttpPost]
        public IActionResult Post([FromBody] FormaPagamentoDTO novaFormaPagamento)
        {
            FormaPagamentoModel formaPagamento = new FormaPagamentoModel(novaFormaPagamento);
            _context.FormaPagamento.Add(formaPagamento);
            _context.SaveChanges();
            
            novaFormaPagamento.setFormaPagamentoId(formaPagamento.FormaPagamentoID);
            return Ok(novaFormaPagamento);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] FormaPagamentoDTO edicoes)
        {
            var formaPagamento = _context.FormaPagamento
                .FirstOrDefault(fp => fp.FormaPagamentoID == id);

            if (formaPagamento == null)
                return NotFound();
            
            formaPagamento = edicoes.editFuncionario(formaPagamento);

            _context.Entry(formaPagamento).CurrentValues.SetValues(formaPagamento);
            _context.SaveChanges();
            return Ok(edicoes);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var formaPagamento = _context.FormaPagamento
                .FirstOrDefault(fp => fp.FormaPagamentoID == id);
            
            if (formaPagamento == null)
            {
                return NotFound();
            }

            _context.FormaPagamento.Remove(formaPagamento);
            _context.SaveChanges();
            return Ok();
        }
    }
}