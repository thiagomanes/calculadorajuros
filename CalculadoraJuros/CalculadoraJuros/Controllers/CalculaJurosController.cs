using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraJuros.Controllers
{
    [ApiController]
    [Route("calculajuros")]
    public class CalculaJurosController : ControllerBase
    {

        const string urlGit = "aquiviraoendereo";

        private decimal Truncar(decimal valor, int precisao)
        {
            decimal step = (decimal)Math.Pow(10, precisao);
            decimal tmp = Math.Truncate(step * valor);
            return tmp / step;
        }


        private readonly ILogger<CalculaJurosController> _logger;

        public CalculaJurosController(ILogger<CalculaJurosController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ObjectResult> Get(decimal valorInicial, int meses)
        {
            try
            {
                decimal taxaJuros = 0.01M;
                var percentual = valorInicial * (valorInicial * decimal.Parse(Math.Pow(double.Parse(taxaJuros.ToString()) + 1, (meses)).ToString()) * taxaJuros- 1);
                var retorno = valorInicial + valorInicial * (percentual / 100);

                return Ok(Truncar(retorno, 2));
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno! Detalhe={ex.Message}");
            }
        }

        [HttpGet]
        [Route("/showmethecode")]
        public ObjectResult showmethecode()
        {
            try
            {
                return Ok(urlGit);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno! Detalhe={ex.Message}");
            }
        }
    }
}
