using Microsoft.AspNetCore.Mvc;
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
            return Ok(result);
        }


    }
}
