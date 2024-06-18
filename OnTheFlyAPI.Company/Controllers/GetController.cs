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
        public async Task<List<Models.Company>> GetAll(int param) => await _getService.GetAll(param);


        [HttpGet("{param},{value}")]
        public async Task<ActionResult<Models.Company>> Get(int param, string value)
        {
            value = value.Replace("+", " ");
            Models.Company company;
            company = await _getService.GetByCnpj(param, value);

            if (company == null)
            {
                company = await _getService.GetByName(param, value);
            }
            if (company == null)
            {
                return NotFound("Companhia nao encontrada");
            }
            return company;
        }
    }
}