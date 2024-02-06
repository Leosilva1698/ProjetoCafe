using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Exceptions;
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
            try
            {
                var adicional = _context.Adicional.ToList();
                List<AdicionalDTO> listAdicionais = new List<AdicionalDTO>();

                foreach (var a in adicional)
                {
                    listAdicionais.Add(new AdicionalDTO(a));
                }

                return Ok(listAdicionais);
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
                var adicional = _context.Adicional
                .FirstOrDefault(a => a.AdicionalID == id);

                if (adicional == null)
                    return NotFound("Adicional não encontrado");
            
                return Ok(new AdicionalDTO(adicional));
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
        public IActionResult Post([FromBody] AdicionalDTO novoAdicional)
        {
            try
            {
                AdicionalModel adicional = new AdicionalModel(novoAdicional);
                _context.Adicional.Add(adicional);
                _context.SaveChanges();
                
                novoAdicional.setAdicionalId(adicional.AdicionalID);
                return Ok(novoAdicional);
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
        public IActionResult Put(int id, [FromBody] AdicionalDTO edicoes)
        {
            try
            {
                var adicional = _context.Adicional
                .FirstOrDefault(a => a.AdicionalID == id);

                if (adicional == null)
                    return NotFound("Adicional não encontrado");
                
                adicional = edicoes.editAdicional(adicional);

                _context.Entry(adicional).CurrentValues.SetValues(adicional);
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
                var adicional = _context.Adicional
                    .FirstOrDefault(a => a.AdicionalID == id);
                
                if (adicional == null)
                {
                    return NotFound("Adicional não encontrado");
                }

                _context.Adicional.Remove(adicional);
                _context.SaveChanges();
                return Ok($"Adicional {adicional.Descricao} excluido!");
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