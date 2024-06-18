using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Company.Models;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.Company.Controllers
{
    [Route("api/Post")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly Post _postService;
        private readonly Get _getService;

        public PostController(Post postService, Get getService)
        {
            _postService = postService;
            _getService = getService;
        }

        [HttpGet]
        public async Task<ActionResult<Models.Company>> Get(Models.Company company)
        {
            return company;
        }

        [HttpPost]
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

                if(dto.Name.Length < 3 || dto.Name == "string")
                    return Problem("Razão Social inválida!");

                if (dto.NameOpt == "" || dto.NameOpt == "string")
                    dto.NameOpt = dto.Name;

                var address = await _getService.RetrieveAdressAPI(dto.Address);
                if (address == null)
                    return Problem("CEP Inválido!");
                
                company = new(dto);
                company.Address = address;
                company.Address.ZipCode = Convert.ToUInt64(company.Address.ZipCode).ToString(@"00\.000\-000");
                var result = await _postService.PostCompany(company);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return CreatedAtAction("Get", company, company);
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

                var address = await _getService.RetrieveAdressAPI(dto.Address);
                if (address == null)
                    return Problem("CEP Inválido!");

                company = new(dto);
                company.Address = address;
                company.Address.ZipCode = Convert.ToUInt64(company.Address.ZipCode).ToString(@"00\.000\-000");
                var result = await _postService.PostHistoryCompany(company);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return CreatedAtAction("Get", company, company);
        }
    }
}
