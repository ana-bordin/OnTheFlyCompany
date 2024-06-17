using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnTheFlyAPI.Company.Models;

namespace OnTheFlyAPI.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PutController : Controller
    {

        [HttpPut("{Cnpj}")]
        public async Task<IActionResult> PutCompany(string cnpj, Models.Company company)
        {
            if (cnpj == company.Cnpj)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(cnpj))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();

        }

        [HttpPut("{Name}")]
        public async Task<IActionResult> PutCompany(string name, Models.Company company)
        {
            if (name == company.Name)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
    }
}