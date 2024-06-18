using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Address.Services;
using OnTheFlyAPI.Company.Models;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly CompanyService _companyService;
        private readonly AddressesService _addressService;
        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost("post")]
        public async Task<ActionResult<Models.Company>> Post(CompanyDTO dto)
        {
            Models.Company company;
            try
            {
                var cnpjaux = string.Join("", dto.Cnpj.Where(Char.IsDigit)); // Somente numeros 
                if (await _companyService.GetByCnpj(0, cnpjaux) != null)
                    return Problem("Company is already registered!!");
                if (await _companyService.GetByCnpj(1, cnpjaux) != null)
                    return Problem("Company is already registered and it is deleted. Restore it if needed.");

                dto.Address.ZipCode = string.Join("", dto.Address.ZipCode.Where(Char.IsDigit)); // Somente numeros
                if (dto.Address.ZipCode.Length != 8)
                    return Problem($"ZipCode ({dto.Address.ZipCode}) different than 8 digits!");

                if (!Models.Company.VerificarCnpj(dto.Cnpj))
                    return Problem("Invalid CNPJ!");

                if (dto.Name.Length < 2 || dto.Name == "string")
                    return Problem("Invalid Name!");

                if (dto.NameOpt == "" || dto.NameOpt == "string")
                    dto.NameOpt = dto.Name;

                var address = await _companyService.RetrieveAdressAPI(dto.Address);
                if (address == null)
                    return Problem("Invalid ZipCode!");

                company = new(dto);
                company.Address = address;
                company.Address.ZipCode = Convert.ToUInt64(company.Address.ZipCode).ToString(@"00\.000\-000");

                var result = await _companyService.PostCompany(company);

                // Add aircraft
                if (result != null)
                {
                    await _companyService.PostAircraft(dto.Cnpj);
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return CreatedAtAction("GetByCnpj", new { param = 0, company.Cnpj }, company);
        }

        [HttpGet("{param}")]
        public async Task<ActionResult<List<Models.Company>>> GetAll(int param)
        {
            try
            {
                var company = await _companyService.GetAll(param);

                if (param != 0 && param != 1)
                {
                    return BadRequest("Parameter must be 0 (Companies without restriction) or 1 (Companies with restriction)");
                }
                if (company.Count == 0)
                {
                    return NotFound("There are no companies registered.");
                }
                return Ok(company);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpGet("cnpj/{param}/{cnpj}")]
        public async Task<ActionResult<Models.Company>> GetByCnpj(int param, string cnpj)
        {
            try
            {
                var company = await _companyService.GetByCnpj(param, cnpj);

                if (param != 0 && param != 1)
                {
                    return BadRequest("Parameter must be 0 (Companies without restriction) or 1 (Companies with restriction)");
                }
                if (company == null)
                {
                    return NotFound("There are no companies registered");
                }
                return Ok(company);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("name/{param}/{name}")]
        public async Task<ActionResult<Models.Company>> GetByName(int param, string name)
        {
            try
            {
                var company = await _companyService.GetByName(param, name);

                if (param != 0 && param != 1)
                {
                    return BadRequest("Parameter must be 0 (Companies without restriction) or 1 (Companies with restriction)");
                }
                if (company == null)
                {
                    return NotFound("There are no companies registered");
                }
                return Ok(company);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPatch("{Cnpj}")]
        public async Task<IActionResult> Patch(Models.CompanyPatchDTO DTO, string Cnpj)
        {
            try
            {
                var result = await _companyService.Update(DTO, Cnpj);
                if (result == null)
                    return Problem("Company not found!");
                if (result.Restricted)
                    return Problem("Company is currently restricted!");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPatch("Status/{Cnpj}")]
        public async Task<IActionResult> PatchStatus(Models.CompanyPatchStatusDTO DTO, string Cnpj)
        {
            try
            {
                var company = await _companyService.GetByCnpj(0, Cnpj);
                
                if (company.Restricted == DTO.Restricted)
                    return Problem("Company status is already " + DTO.Restricted);
                
                var result = await _companyService.UpdateStatus(DTO, Cnpj);
                if (result == null)
                    return Problem("Company not found!");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("delete/{cnpj}")]
        public async Task<ActionResult<Models.Company>> Delete(string cnpj)
        {
            try
            {
                var company = await _companyService.GetByCnpj(0, cnpj);

                if (company == null)
                    return NotFound("Company not found!");

                var inserted = await _companyService.PostHistoryCompany(company);

                if (inserted == null)
                    return BadRequest("There was a problem moving the company.");

                var deleted = await _companyService.DeleteCompany(cnpj);

                if (deleted == false)
                    throw new Exception("There was a problem deleting the company");

                return Ok("Company successfully deleted!");

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("restorage/{cnpj}")]
        public async Task<ActionResult<Models.Company>> Restorage(string cnpj)
        {
            try
            {
                var company = await _companyService.GetByCnpj(1, cnpj);

                if (company == null)
                    return NotFound("Company not found!");

                var inserted = await _companyService.PostCompany(company);

                if (inserted == null)
                    return BadRequest("There was a problem moving the company");

                var deleted = await _companyService.RestorageCompany(cnpj);

                if (deleted == null)
                    throw new Exception("There was a problem restoring the company");

                return Ok("Company successfully restored!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
