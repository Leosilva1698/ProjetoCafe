using Microsoft.AspNetCore.Mvc;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;
using ProjetoCafe.Exceptions;

namespace ProjetoCafe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly CafeDBContext _context;

        public FuncionarioController()
        {
            _context = new CafeDBContext();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var funcionarios = _context.Funcionario.ToList();
                List<FuncionarioDTO> listFucionarios = new List<FuncionarioDTO>();

                foreach (var f in funcionarios)
                {
                    listFucionarios.Add(new FuncionarioDTO(f));
                }

                return Ok(listFucionarios);
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
                var funcionario = _context.Funcionario
                .FirstOrDefault(f => f.FuncionarioID == id);

                if (funcionario == null)
                    return NotFound("Funcionario não encontrado.");

                return Ok(new FuncionarioDTO(funcionario));
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

        [HttpGet("Gorjetas/{notafiscalId}")]
        public IActionResult funcionariosGorjeta(int notafiscalId)
        {
            try
            {
                var notafiscal = _context.NotaFiscal.FirstOrDefault(nf => nf.NotaFiscalID == notafiscalId);

                if (notafiscal == null)
                    return NotFound("Nota Fiscal não encontrada");



                var funcionarios = _context.NotaFiscal
                    .Where(nf => nf.NotaFiscalID == notafiscalId)
                    .Join(
                        _context.Comanda,
                        nota => nota.ComandaID,
                        comanda => comanda.ComandaID,
                        (nota, comanda) => new { comanda }
                    )
                    .Join(
                        _context.Pedido,
                        comanda => comanda.comanda.ComandaID,
                        pedido => pedido.ComandaID,
                        (comanda, pedido) => new { pedido }
                    )
                    .Join(
                        _context.Funcionario,
                        pedido => pedido.pedido.FuncionarioID,
                        funcionario => funcionario.FuncionarioID,
                        (pedido, funcionario) => new { funcionario.Nome }
                    )
                    .Select(resultado => resultado.Nome)
                    .ToList();

                int quantFuncionarios = funcionarios.Count;

                string display = "";
                if (notafiscal.TaxaServico)
                {
                    double gorjeta = notafiscal.ValorTotal * 0.1 / quantFuncionarios;

                    foreach (var nome in funcionarios)
                    {
                        display += $"{nome} recebeu {gorjeta} R$ de gorjeta\n";
                    }
                }
                else
                {
                    display += "Essa nota não gerou gorgeta";
                }
                return Ok(display);
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
        public IActionResult Post([FromBody] FuncionarioDTO novoFuncionario)
        {
            try
            {
                FuncionarioModel funcionario = new FuncionarioModel(novoFuncionario);
                _context.Funcionario.Add(funcionario);
                _context.SaveChanges();

                novoFuncionario.setFuncionarioId(funcionario.FuncionarioID);
                return Ok(novoFuncionario);
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
        public IActionResult Put(int id, [FromBody] FuncionarioDTO edicoes)
        {
            try
            {
                var funcionario = _context.Funcionario
                    .FirstOrDefault(f => f.FuncionarioID == id);

                if (funcionario == null)
                    return NotFound("Funcionario não encontrado");

                funcionario = edicoes.editFuncionario(funcionario);

                _context.Entry(funcionario).CurrentValues.SetValues(funcionario);
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
                var funcionario = _context.Funcionario
                    .FirstOrDefault(f => f.FuncionarioID == id);

                if (funcionario == null)
                {
                    return NotFound("Funcionario não encontrado");
                }

                _context.Funcionario.Remove(funcionario);
                _context.SaveChanges();
                return Ok($"Funcionario {funcionario.Nome} excluido!");
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