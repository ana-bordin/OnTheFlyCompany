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
                dto.Address.ZipCode = string.Join("", dto.Address.ZipCode.Where(Char.IsDigit)); // Formata pra vir apenas números do CEP
                if (dto.Address.ZipCode.Length != 8)
                    return Problem($"CEP ({dto.Address.ZipCode}) diferente de 8 dígitos!");

                if (!Models.Company.VerificarCnpj(dto.Cnpj))
                    return Problem("CNPJ Inválido!");

                if (dto.Name.Length < 3 || dto.Name == "string")
                    return Problem("Razão Social inválida!");

                if (dto.NameOpt == "" || dto.NameOpt == "string")
                    dto.NameOpt = dto.Name;

                var address = await _companyService.RetrieveAdressAPI(dto.Address);
                if (address == null)
                    return Problem("CEP Inválido!");

                company = new(dto);
                company.Address = address;
                company.Address.ZipCode = Convert.ToUInt64(company.Address.ZipCode).ToString(@"00\.000\-000");

                var result = await _companyService.PostCompany(company);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return CreatedAtAction("GetByCnpj", new { param = 0, company.Cnpj }, company);
        }

        [HttpPost("history")]
        public async Task<ActionResult<Models.Company>> PostHistory(CompanyDTO dto)
        {
            Models.Company company;
            try
            {
                dto.Address.ZipCode = string.Join("", dto.Address.ZipCode.Where(Char.IsDigit)); // Formata pra vir apenas números do CEP
                if (dto.Address.ZipCode.Length != 8)
                    return Problem($"CEP ({dto.Address.ZipCode}) diferente de 8 dígitos!");

                if (!Models.Company.VerificarCnpj(dto.Cnpj))
                    return Problem("CNPJ Inválido!");

                if (dto.Name.Length < 3 || dto.Name == "string")
                    return Problem("Razão Social inválida!");

                if (dto.NameOpt == "" || dto.NameOpt == "string")
                    dto.NameOpt = dto.Name;

                var address = await _companyService.RetrieveAdressAPI(dto.Address);
                if (address == null)
                    return Problem("CEP Inválido!");

                company = new(dto);
                company.Address = address;
                company.Address.ZipCode = Convert.ToUInt64(company.Address.ZipCode).ToString(@"00\.000\-000");

                var result = await _companyService.PostHistoryCompany(company);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return CreatedAtAction("GetByCnpj", new { param = 1, company.Cnpj }, company);
        }

        [HttpGet("{param}")]
        public async Task<ActionResult<List<Models.Company>>> GetAll(int param)
        {
            try
            {
                var company = await _companyService.GetAll(param);

                if (param != 0 && param != 1)
                {
                    return BadRequest("Parametro deve ser 0 (Companhias sem restricao) ou 1 (Companhias com restricao)");
                }
                if (company.Count == 0)
                {
                    return NotFound("Nao ha companhias cadastradas");
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
                    return BadRequest("Parametro deve ser 0 (Companhias sem restricao) ou 1 (Companhias com restricao)");
                }
                if (company == null)
                {
                    return NotFound("Companhia nao encontrada");
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
                    return BadRequest("Parametro deve ser 0 (Companhias sem restricao) ou 1 (Companhias com restricao)");
                }
                if (company == null)
                {
                    return NotFound("Companhia nao encontrada");
                }
                return Ok(company);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPatch("{Cnpj}")]
        public async Task<IActionResult> Put(Models.CompanyPatchDTO DTO, string Cnpj)
        {
            try
            {
                var result = await _companyService.Update(DTO, Cnpj);
                if (result == null)
                    return Problem("Companhia não encontrada!");
                if (result.Restricted)
                    return Problem("Companhia encontra-se restrita!");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPatch("Status/{Cnpj}")]
        public async Task<IActionResult> PutStatus(Models.CompanyPatchStatusDTO DTO, string Cnpj)
        {
            try
            {
                var result = await _companyService.UpdateStatus(DTO, Cnpj);
                if (result == null)
                    return Problem("Companhia não encontrada!");

                //todo: fazer get status

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
                    return NotFound("Companhia não encontrada!");

                var inserted = await _companyService.PostHistoryCompany(company);

                if (inserted == null)
                    return BadRequest("Houve um problema para mover a companhia");

                var deleted = await _companyService.DeleteCompany(cnpj);

                if (deleted == false)
                    throw new Exception("Houve um problema para deletar a companhia");

                return Ok("Deletado com sucesso!");
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
                    return NotFound("Companhia não encontrada!");

                var inserted = await _companyService.PostCompany(company);

                if (inserted == null)
                    return BadRequest("Houve um problema para mover a companhia");

                var deleted = await _companyService.RestorageCompany(cnpj);

                if (deleted == null)
                    throw new Exception("Houve um problema para restaurar a companhia");

                return Ok("Restaurado com sucesso!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
