using Microsoft.AspNetCore.Mvc;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
using ProjetoCafe.Models;

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
            var funcionarios = _context.Funcionario.ToList();
            List<FuncionarioDTO> listFucionarios = new List<FuncionarioDTO>();

            foreach (var f in funcionarios)
            {
                listFucionarios.Add(new FuncionarioDTO(f));
            }

            return Ok(listFucionarios);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var funcionario = _context.Funcionario
                .FirstOrDefault(f => f.FuncionarioID == id);

            if (funcionario == null)
                return NotFound();
        
            return Ok(new FuncionarioDTO(funcionario));
        }

        [HttpGet("Gorjetas/{notafiscalId}")]
        public IActionResult funcionariosGorjeta(int notafiscalId)
        {
            var notafiscal = _context.NotaFiscal.FirstOrDefault(nf => nf.NotaFiscalID == notafiscalId);

            if (notafiscal == null)
                return NotFound();

            var funcionarios = _context.NotaFiscal
                .Where(nf => nf.NotaFiscalID == notafiscalId)
                .Join(
                    _context.Comanda,
                    nota => nota.ComandaID,
                    comanda => comanda.ComandaID,
                    (nota, comanda) => new {comanda}
                )
                .Join(
                    _context.Pedido,
                    comanda => comanda.comanda.ComandaID,
                    pedido => pedido.ComandaID,
                    (comanda, pedido) => new {pedido}
                )
                .Join(
                    _context.Funcionario,
                    pedido => pedido.pedido.FuncionarioID,
                    funcionario => funcionario.FuncionarioID,
                    (pedido, funcionario) => new {funcionario.Nome}
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

            return Ok(display);
        }

        [HttpPost]
        public IActionResult Post([FromBody] FuncionarioDTO novoFuncionario)
        {
            FuncionarioModel funcionario = new FuncionarioModel(novoFuncionario);
            _context.Funcionario.Add(funcionario);
            _context.SaveChanges();
            
            novoFuncionario.setFuncionarioId(funcionario.FuncionarioID);
            return Ok(novoFuncionario);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] FuncionarioDTO edicoes)
        {
            var funcionario = _context.Funcionario
                .FirstOrDefault(f => f.FuncionarioID == id);

            if (funcionario == null)
                return NotFound();
            
            funcionario = edicoes.editFuncionario(funcionario);

            _context.Entry(funcionario).CurrentValues.SetValues(funcionario);
            _context.SaveChanges();
            return Ok(edicoes);
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
                    return NotFound();
                }

                _context.Funcionario.Remove(funcionario);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}