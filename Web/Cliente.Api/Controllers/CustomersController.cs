using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vittoria.Cliente.Api;
using Vittoria.Infrastructure.CorrelationId;

namespace Vittoria.Cliente.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public CustomersController(ILogger<CustomersController> logger, ICorrelationContextAccessor correlationContextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _correlationContextAccessor = correlationContextAccessor;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            var correlationId = _correlationContextAccessor.Context.CorrelationId;
            _logger.LogInformation($"GET METHOD: {correlationId}");
            return Ok(new { Id = id });
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post([FromBody] Customer model)
        {
            _logger.LogInformation("POST METHOD");
            return NoContent();
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search([FromQuery] string name, [FromQuery] string lastName)
        {
            _logger.LogInformation("SEARCH METHOD");

            try
            {
                if (string.IsNullOrEmpty(lastName))
                {
                    throw new ArgumentException("Parametro LastName obbligatorio", nameof(lastName));
                }

                return Ok(new { Name = name });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                throw ex;
            }

            
        }
    }
}
