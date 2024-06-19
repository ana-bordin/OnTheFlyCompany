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
                var cnpjaux = Models.Company.RemoveMask(dto.Cnpj);
                if (await _companyService.GetByCnpj(0, cnpjaux) != null)
                    return BadRequest("Company is already registered!!");
                if (await _companyService.GetByCnpj(1, cnpjaux) != null)
                    return BadRequest("Company is already registered and it is deleted. Restore it if needed.");

                dto.Address.ZipCode = Address.Models.Address.RemoveMask(dto.Address.ZipCode);

                if (dto.Address.ZipCode.Length != 8)
                    return BadRequest($"ZipCode ({dto.Address.ZipCode}) different than 8 digits!");

                if (!Models.Company.VerifyCNPJ(dto.Cnpj))
                    return BadRequest("Invalid CNPJ!");

                if (dto.Name.Length < 2 || dto.Name == "string")
                    return BadRequest("Name too short!");

                if (dto.Name.Length > 30)
                    return BadRequest("Name too long!");

                if (dto.DtOpen > DateTime.Now)
                    return BadRequest("Date of opening cannot be newer than current date!");

                if (dto.NameOpt == "" || dto.NameOpt == "string")
                    dto.NameOpt = dto.Name;

                var address = await _companyService.RetrieveAdressAPI(dto.Address);

                if (address == null)
                    return BadRequest("Invalid ZipCode!");

                company = new(dto);
                company.Address = address;
                company.Address.ZipCode = Address.Models.Address.RemoveMask(company.Address.ZipCode);

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

            return Ok(company);
            //return CreatedAtAction("GetByCnpj", new { param = 0, company.Cnpj }, company);
        }

        [HttpGet("{param}")]
        public async Task<ActionResult<List<Models.Company>>> GetAll(int param)
        {
            try
            {
                var company = await _companyService.GetAll(param);

                if (param != 0 && param != 1)
                {
                    return BadRequest("Parameter must be 0 (Companies registered) or 1 (Companies excludeds)");
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
                    return BadRequest("Parameter must be 0 (Companies registered) or 1 (Companies excludeds)");
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
                    return BadRequest("Parameter must be 0 (Companies registered) or 1 (Companies excludeds)");
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
        public async Task<IActionResult> Patch(CompanyPatchDTO DTO, string Cnpj)
        {
            try
            {
                var company = await _companyService.GetByCnpj(0, Cnpj);

                if (company == null)
                    return BadRequest("Company not found!");

                if (company.Restricted)
                    return Problem("Company is currently restricted!");

                // If name dto is empty then receives the company name
                if (DTO.NameOpt == "")
                    DTO.NameOpt = company.NameOpt;

                // If street dto is empty then receives the company address
                if (company.Address.Street != "")
                    DTO.Street = company.Address.Street;

                // If number dto is 0 then receives the company number
                if (DTO.Number == 0)
                    DTO.Number = company.Address.Number;
                
                if (DTO.Street.Length > 100)
                    return BadRequest("Street too long!");

                if (DTO.NameOpt.Length > 30)
                    return BadRequest("Name too long!");

                if (DTO.Complement.Length > 10)
                    return BadRequest("Complement too long!");

                var result = await _companyService.Update(DTO, Cnpj);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPatch("Status/{Cnpj}")]
        public async Task<IActionResult> PatchStatus(CompanyPatchStatusDTO DTO, string Cnpj)
        {
            try
            {
                var company = await _companyService.GetByCnpj(0, Cnpj);

                if (company.Restricted == DTO.Restricted)
                    return BadRequest("Company status is already " + DTO.Restricted);

                var result = await _companyService.UpdateStatus(DTO, Cnpj);
                if (result == null)
                    return BadRequest("Company not found!");

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
                    return BadRequest("Company not found!");

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
                    return BadRequest("Company not found!");

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

        public async Task Patch(CompanyDTO companyDTO)
        {
            throw new NotImplementedException();
        }
    }
}
