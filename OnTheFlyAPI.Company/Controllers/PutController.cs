using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.Company.Controllers
{
    public class PutController : Controller
    {
        private readonly Put _putService;
        public PutController(Put putService)
        {
            _putService = putService;
        }

        [HttpPut("{Cnpj}")]
        public async Task<IActionResult> Put(Models.Company company)
        { 
            _putService.Update(company);
            return Ok(company);
        }

        
    }
}
