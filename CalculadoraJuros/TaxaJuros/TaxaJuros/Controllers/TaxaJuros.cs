using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxaJuros.Controllers
{
    [ApiController]
    [Route("taxajuros")]
    [AllowAnonymous]
    public class TaxaJuros : ControllerBase
    {
      

        private readonly ILogger<TaxaJuros> _logger;

        public TaxaJuros(ILogger<TaxaJuros> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        
        public async Task<ObjectResult> Get()
        {
            try
            {
              
                return Ok(0.01);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno! Detalhe={ex.Message}");
            }
        }
    }
}
