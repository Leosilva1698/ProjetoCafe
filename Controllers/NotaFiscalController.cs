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
    public class NotaFiscalController : ControllerBase
    {
        private readonly CafeDBContext _context;

        public NotaFiscalController()
        {
            _context = new CafeDBContext();
        }

        void GerarCashBack(int ClienteId, int NotaFiscalId)
        {
            var notaFiscal = _context.NotaFiscal.FirstOrDefault(nf => nf.NotaFiscalID == NotaFiscalId) ?? throw new CafeException("Nota fiscal não encontrada");
            var cliente = _context.Cliente.FirstOrDefault(c => c.ClienteID == ClienteId) ?? throw new CafeException("Cliente não encontrado");

            DateTime dataHora = DateTime.Now;

            CupomClienteModel novoCupom = new CupomClienteModel(ClienteId, NotaFiscalId, dataHora, dataHora.AddDays(14), notaFiscal!.ValorTotal * .1, true);

            _context.CupomCliente.Add(novoCupom);
            _context.SaveChanges();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var notasFiscais = _context.NotaFiscal.ToList();
                List<NotaFiscalDTO> listNotasFiscais = new List<NotaFiscalDTO>();

                foreach (var nf in notasFiscais)
                {
                    listNotasFiscais.Add(new NotaFiscalDTO(nf));
                }

                return Ok(listNotasFiscais);
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
                var notasFiscais = _context.NotaFiscal
                .FirstOrDefault(nf => nf.NotaFiscalID == id);

                if (notasFiscais == null)
                    return NotFound("Nota fiscal não encontrada");

                return Ok(new NotaFiscalDTO(notasFiscais));
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
        public IActionResult Post([FromBody] NotaFiscalDTO novaNotaFiscal)
        {
            try
            {

                var FormaPagamento = _context.FormaPagamento.FirstOrDefault(fp => fp.FormaPagamentoID == novaNotaFiscal.FormaPagamentoID);
                if (FormaPagamento == null)
                    return NotFound("Forma de pagamento não encontrada");
                var comanda = _context.Comanda
                    .FirstOrDefault(c => c.ComandaID == novaNotaFiscal.ComandaID);
                if (comanda == null)
                    return NotFound("Comanda não encontrada");

                NotaFiscalModel notaFiscal = new NotaFiscalModel(novaNotaFiscal);

                var pedidoValores = _context.Pedido
                    .Where(p => p.ComandaID == novaNotaFiscal.ComandaID)
                    .Select(p => p.ValorTotal)
                    .ToList();

                foreach (var valor in pedidoValores)
                {
                    notaFiscal.ValorTotal += valor;
                }
                DateTime dataAtual = DateTime.Now;
                var cupom = _context.CupomCliente
                    .OrderBy(c => c.NotaFiscalID)
                    .FirstOrDefault(
                        c => c.ClienteID == novaNotaFiscal.ClienteID &&
                        c.Valor == novaNotaFiscal.Desconto &&
                        c.Valido == true &&
                        c.DataValidade < dataAtual
                    );

                if (cupom == null)
                {
                    notaFiscal.Desconto = 0;
                }
                else
                {
                    cupom.Valido = false;
                    _context.Entry(cupom).CurrentValues.SetValues(cupom);
                }

                notaFiscal.ValorFinal = notaFiscal.TaxaServico == true
                    ? notaFiscal.ValorTotal * 1.1 - notaFiscal.Desconto
                    : notaFiscal.ValorTotal - notaFiscal.Desconto;

                comanda.EstaAberta = false;
                _context.Entry(comanda).CurrentValues.SetValues(comanda);
                _context.NotaFiscal.Add(notaFiscal);
                _context.SaveChanges();

                if (notaFiscal.ValorTotal > 50 && novaNotaFiscal.ClienteID != 0)
                {
                    GerarCashBack(novaNotaFiscal.ClienteID, notaFiscal.NotaFiscalID);
                }

                novaNotaFiscal.setNotaFiscalValues(notaFiscal);
                return Ok(novaNotaFiscal);
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
            catch (DbUpdateException e)
            {
                _ = new CafeException(e.Message);
                return BadRequest("Erro ao persistir no Banco de Dados. Certifique-se que já não foi gerado uma nota fiscal para a comanda com o ID informado");

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
                var notaFiscal = _context.NotaFiscal
                .FirstOrDefault(nf => nf.NotaFiscalID == id);

                if (notaFiscal == null)
                    return NotFound("Nota fiscal nao encontrada");

                _context.NotaFiscal.Remove(notaFiscal);
                _context.SaveChanges();
                return Ok($"Nota fiscal {id} excluida");

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