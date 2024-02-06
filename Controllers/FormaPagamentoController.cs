using Microsoft.AspNetCore.Mvc;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;
using ProjetoCafe.Exceptions;

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
            try
            {
                var formasPagamento = _context.FormaPagamento.ToList();
                List<FormaPagamentoDTO> listFormaPagemento = new List<FormaPagamentoDTO>();

                foreach (var fp in formasPagamento)
                {
                    listFormaPagemento.Add(new FormaPagamentoDTO(fp));
                }

                return Ok(listFormaPagemento);
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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var formaPagamento = _context.FormaPagamento
                .FirstOrDefault(fp => fp.FormaPagamentoID == id);

                if (formaPagamento == null)
                    return NotFound("Forma de pagamento não encontrada");

                return Ok(new FormaPagamentoDTO(formaPagamento));
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
        public IActionResult Post([FromBody] FormaPagamentoDTO novaFormaPagamento)
        {
            try
            {
                FormaPagamentoModel formaPagamento = new FormaPagamentoModel(novaFormaPagamento);
                _context.FormaPagamento.Add(formaPagamento);
                _context.SaveChanges();

                novaFormaPagamento.setFormaPagamentoId(formaPagamento.FormaPagamentoID);
                return Ok(novaFormaPagamento);
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
        public IActionResult Put(int id, [FromBody] FormaPagamentoDTO edicoes)
        {
            try
            {
                var formaPagamento = _context.FormaPagamento
                .FirstOrDefault(fp => fp.FormaPagamentoID == id);

                if (formaPagamento == null)
                    return NotFound("Forma de pagamento não encontrada");

                formaPagamento = edicoes.editFuncionario(formaPagamento);

                _context.Entry(formaPagamento).CurrentValues.SetValues(formaPagamento);
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
                var formaPagamento = _context.FormaPagamento
                .FirstOrDefault(fp => fp.FormaPagamentoID == id);

                if (formaPagamento == null)
                {
                    return NotFound("Forma de pagamento não encontrada");
                }

                _context.FormaPagamento.Remove(formaPagamento);
                _context.SaveChanges();
                return Ok($"Forma de pagamento {formaPagamento.Descricao} excluida");
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