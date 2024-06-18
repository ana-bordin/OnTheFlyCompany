using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.Company.Controllers
{
    [Route("api/update")]
    [ApiController]
    public class PutController : Controller
    {
        private readonly Put _putService;
        public PutController(Put putService)
        {
            _putService = putService;
        }

        [HttpPatch("{Cnpj}")]
        public async Task<IActionResult> Put(Models.CompanyPatchDTO DTO, string Cnpj)
        {
            var result = await _putService.Update(DTO, Cnpj);
            if (result == null)
                return Problem("Companhia não encontrada!");
            if (result.Restricted)
                return Problem("Companhia encontra-se restrita!");
            return Ok(result);
        }

        [HttpPatch("Status/{Cnpj}")]
        public async Task<IActionResult> PutStatus(Models.CompanyPatchStatusDTO DTO, string Cnpj)
        {

            var result = await _putService.UpdateStatus(DTO, Cnpj);
            if (result == null)
                return Problem("Companhia não encontrada!");

            //todo: fazer get status

            return Ok(result);
        }
    }
}
