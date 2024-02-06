using Microsoft.AspNetCore.Mvc;
using ProjetoCafe.Data;
using ProjetoCafe.DTOS;
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
            var notaFiscal = _context.NotaFiscal.FirstOrDefault(nf => nf.NotaFiscalID == NotaFiscalId);
            var cliente = _context.Cliente.FirstOrDefault(c => c.ClienteID == ClienteId);

            // if (cliente == null)
            // {
            //     // throw new cafeExp
            // }

            DateTime dataHora = DateTime.Now;

            CupomClienteModel novoCupom = new CupomClienteModel(ClienteId, NotaFiscalId, dataHora, dataHora.AddDays(14), notaFiscal!.ValorTotal * .1, true);

            _context.CupomCliente.Add(novoCupom);
            _context.SaveChanges();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var notasFiscais = _context.NotaFiscal.ToList();
            List<NotaFiscalDTO> listNotasFiscais = new List<NotaFiscalDTO>();
            
            foreach (var nf in notasFiscais)
            {
                listNotasFiscais.Add(new NotaFiscalDTO(nf));
            }

            return Ok(listNotasFiscais);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var notasFiscais = _context.NotaFiscal
                .FirstOrDefault(nf => nf.NotaFiscalID == id);
            
            if (notasFiscais == null)
                return NotFound();
                
            return Ok(new NotaFiscalDTO(notasFiscais));
        }

        [HttpPost]
        public IActionResult Post([FromBody] NotaFiscalDTO novaNotaFiscal)
        {
            NotaFiscalModel notaFiscal = new NotaFiscalModel(novaNotaFiscal);

            var comanda = _context.Comanda
                .FirstOrDefault(c => c.ComandaID == novaNotaFiscal.ComandaID);

            if (comanda == null)
                return NotFound();

            var pedidoValores = _context.Pedido
                .Where(p => p.ComandaID == novaNotaFiscal.ComandaID)
                .Select(p => p.ValorTotal)    
                .ToList();

            foreach (var valor in pedidoValores)
            {
                notaFiscal.ValorTotal += valor;
            }

            var cupom = _context.CupomCliente
                .OrderBy(c => c.NotaFiscalID)
                .FirstOrDefault (
                    c => c.ClienteID == novaNotaFiscal.ClienteID &&
                    c.Valor == novaNotaFiscal.Desconto &&
                    c.Valido == true
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
                ?   notaFiscal.ValorTotal * 1.1 - notaFiscal.Desconto
                :   notaFiscal.ValorTotal - notaFiscal.Desconto;

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


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var notaFiscal = _context.NotaFiscal
                .FirstOrDefault(nf => nf.NotaFiscalID == id);

            if (notaFiscal == null)
                return NotFound();

            _context.NotaFiscal.Remove(notaFiscal);
            _context.SaveChanges();
            return Ok();
        } 
    }
}