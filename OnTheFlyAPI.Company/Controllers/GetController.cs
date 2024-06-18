using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetController : Controller
    {
        private readonly Get _getService;

        public GetController(Get getService)
        {
            _getService = getService;
        }

        [HttpGet("{param}")]
        public async Task<ActionResult<List<Models.Company>>> GetAll(int param)
        {
            var company = await _getService.GetAll(param);

            if (param != 0 && param != 1)
            {
                return BadRequest("Parametro deve ser 0 ou 1");
            }
            if (company.Count == 0)
            {
                return BadRequest("Nao ha companhias cadastradas");
            }
            return Ok(company);
        }


        [HttpGet("cnpj/{param}/{cnpj}")]
        public async Task<ActionResult<Models.Company>> GetByCnpj(int param, string cnpj)
        {
            var company = _getService.GetByCnpj(param, cnpj);

            if (param != 0 && param != 1)
            {
                return BadRequest("Parametro deve ser 0 ou 1");
            }
            if (company == null)
            {
                return NotFound("Companhia nao encontrada");
            }
            return Ok(company);
        }

        [HttpGet("name/{param}/{name}")]
        public async Task<ActionResult<Models.Company>> GetByName(int param, string name)
        {
            var company = await _getService.GetByName(param, name);

            if (param != 0 && param != 1)
            {
                return BadRequest("Parametro deve ser 0 ou 1");
            }
            if (company == null)
            {
                return NotFound("Companhia nao encontrada");
            }
            return Ok(company);
        }
    }
}