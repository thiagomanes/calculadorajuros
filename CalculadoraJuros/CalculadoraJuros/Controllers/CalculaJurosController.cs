using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CalculadoraJuros.Controllers
{
    [ApiController]
    [Route("calculajuros")]
    public class CalculaJurosController : ControllerBase
    {

        const string urlGit = "https://github.com/thiagomanes/calculadorajuros.git";
        private readonly IConfiguration _configuration;

        private decimal Truncar(decimal valor, int precisao)
        {
            decimal step = (decimal)Math.Pow(10, precisao);
            decimal tmp = Math.Truncate(step * valor);
            return tmp / step;
        }


        private readonly ILogger<CalculaJurosController> _logger;

        public CalculaJurosController(ILogger<CalculaJurosController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        private async Task<decimal> ObterTaxaJuros() 
        {


            var urlAPi = _configuration.GetValue<string>("Configuracao:urlApiTaxaJuros", @"https://localhost:49161/TaxaJuros");


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:49161/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            decimal taxa = 0M;

            HttpResponseMessage response = await client.GetAsync(urlAPi);
            if (response.IsSuccessStatusCode)
            {
                var taxaStr = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(taxaStr))
                    taxa = decimal.Parse(taxaStr);

            }
            return taxa;
         
        }

        [HttpGet]
        public async Task<ObjectResult> Get(decimal valorInicial, int meses)
        {
            try
            {
                decimal taxaJuros = await ObterTaxaJuros();
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
